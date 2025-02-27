using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Microsoft.Extensions.Configuration;
using BlogApp.Common.Logging;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.Elasticsearch;
using BlogApp.Common.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;

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
                // Elasticsearch'e yazma
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

            // HealthCheck servisi ekleme
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy("Servis çalışıyor"), new[] { "api" });
                
            // API Versiyonlama ekleme - şimdilik devre dışı
            /*
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-API-Version"),
                    new QueryStringApiVersionReader("api-version"));
            });
            
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            */
            
            // Caching mekanizması ekleme
            services.AddMemoryCache();
            
            // Distributed memory cache kullan (Redis yerine)
            services.AddDistributedMemoryCache();
            
            // Response caching middleware
            services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024; // 1KB
                options.UseCaseSensitivePaths = false;
            });

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
            
            // Response caching middleware
            app.UseResponseCaching();
            
            // Cache-Control header ekleme
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl = 
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = 
                    new string[] { "Accept-Encoding" };

                await next();
            });
            
            // HealthCheck endpoint'i ekleme
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    
                    var response = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new
                        {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description,
                            duration = e.Value.Duration.ToString()
                        })
                    };
                    
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
            
            return app;
        }
    }
}
