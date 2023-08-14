using ConsulClientDemo;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFakeApplication<ConsulClientDemoModule>();
builder.Configuration.AddConfigurationFromConsul();
var app = builder.Build();
app.InitializeApplication();
app.MapGet("/", () => "Hello World!");
app.MapGet("/health", ([FromServices]ILogger<Program> logger) => logger.LogInformation("健康检查"));

app.Run();