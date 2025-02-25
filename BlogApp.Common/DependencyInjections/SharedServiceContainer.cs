using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.Extensions.Configuration;
using BlogApp.Common.Logging;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.Elasticsearch;
using BlogApp.Common.Middlewares;

namespace BlogApp.Common.DependencyInjections
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticUri = "https://localhost:9200";

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                // Konsola yazma
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3} {SourceContext}] " +
                    "{Message:lj}{NewLine}{Exception}")
                // Elasticsearch’e yazma
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUri))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat = "Gama-logs-{0:yyyy.MM.dd}",
                        ModifyConnectionSettings = x => x.BasicAuthentication("new_user", "123456789")
                                .ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)
                    })
                .CreateLogger();

            Log.Information("Serilog Elasticsearch installation completed.");

            services.AddJWTAuthScheme(configuration);


            //services.AddLogging(builder =>
            //{
            //    builder.ClearProviders();
            //    builder.AddSerilog(Log.Logger, dispose: true);
            //});

            return services;
        }

        public static IApplicationBuilder UseCommonPolicies(this IApplicationBuilder app)
        {
            //app.UseMiddleware<GlobalException>();
            //app.UseMiddleware<RateLimitingMiddleware>();
            return app;

        }
    }
}
