using DesafioTecnicoArtycs.Infra;
using DesafioTecnicoArtycs.CrossCutting.IoC;
using DesafioTecnicoArtycs.Domain.Interfaces.Services;
using System.Net;
using Microsoft.EntityFrameworkCore;
using DesafioTecnicoArtycs.Api.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using DesafioTecnicoArtycs.Domain.Util;
using Microsoft.AspNetCore.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using FluentValidation.AspNetCore;
using DesafioTecnicoArtycs.Domain.Validators;
using DesafioTecnicoArtycs.Domain.Entities;
using DesafioTecnicoArtycs.Domain.Dto.Request;
using DesafioTecnicoArtycs.Application.AutoMapper;

var builder = WebApplication.CreateBuilder(args);




IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Curso Api", Version = "v1", Contact = new OpenApiContact() { 
    Name = "Jo�o",
    Email = "Joaomarcelo@teste.com"
    
    },
    Description = "Web Api para exemplo de sistema"
    });


    c.EnableAnnotations();
    c.AddSecurityDefinition("apikey", new OpenApiSecurityScheme
    {
        Name = "apikey",
        In = ParameterLocation.Header,
        Scheme = "apikey",
        Type = SecuritySchemeType.ApiKey,
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "apikey" }
            },
            new List<string>()
        }
    });


});


builder.Services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

builder.Services
  .AddDbContext<DataContext>(options => options.UseSqlite(settings.ConnectionString)
  .EnableDetailedErrors(true));

builder.Services.RegisterDatabase("");
builder.Services.AddSharedServices();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers()
    .AddFluentValidation();

builder.Services.AddTransient<IValidator<CursoRequest>, CursoValidator>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CursoApi v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


//app.Logger.LogInformation("Adding Routes");
//app.MapGet("/EstaAtivo", () => "Est� ativo");
//app.Logger.LogInformation("Starting the app");

app.Run();
