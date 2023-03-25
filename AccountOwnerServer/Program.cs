using AccountOwnerServer;
using AccountOwnerServer.Extentsions;
using AutoMapper;
using Contracts;
using LoggerService;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

builder.Services.ConfigureCors();

builder.Services.ConfigureIISIntegration();

builder.Services.ConfigureLoggerService();
builder.Services.ConfigureMySqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryWrapper();
//var Config = new MapperConfiguration(con =>
//{
//    con.AddProfile( new MappingProfile());
//});
//var Mappper = Config.CreateMapper();
//builder.Services.AddSingleton(Mappper);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
