using BlogApp.DataApi.Data;
using BlogApp.DataApi.Repositories;
using BlogApp.DataApi.Repositories.Interfaces;
using BlogApp.DataApi.Services;
using BlogApp.DataApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
#region Database Connection

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Repository Services
builder.Services.AddScoped<IExperiencesRepository, ExperiencesRepository>();
builder.Services.AddScoped<IPersonalInfoRepository, PersonalInfoRepository>();
builder.Services.AddScoped<IAboutMeRepository, AboutMeRepository>();
builder.Services.AddScoped<IContactMessagesRepository, ContactMessagesRepository>();
builder.Services.AddScoped<IEducationsRepository, EducationsRepository>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
#endregion

#region Business Services
builder.Services.AddScoped<IAboutMeService, AboutMeService>();
builder.Services.AddScoped<IContactMessagesService, ContactMessagesService>();
builder.Services.AddScoped<IEducationsService, EducationsService>();
builder.Services.AddScoped<IExperiencesService, ExperiencesService>();
builder.Services.AddScoped<IPersonalInfoService, PersonalInfoService>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
