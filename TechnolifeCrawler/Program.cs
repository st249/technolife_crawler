using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Serilog;
using System.Configuration;
using TechnolifeCrawler.Infrastructure.DataAccess;
using TechnolifeCrawler.StartupExtentions;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
// Add services to the container.



Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .WriteTo.Console());

builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

builder.Services.AddDbContext<TechnolifeCrawlerDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("Default")));

builder.Services.AddConfiguredMassTransit(configuration);
builder.Services.AddServices();
builder.Services.AddHostedServices();
builder.Services.AddConfigurations(configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();