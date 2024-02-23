var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication<ConsulClientDemoModule>();
builder.Configuration.AddConfigurationFromConsul();
var app = builder.Build();
app.InitializeApplication();
app.MapGet("/", () => "Hello World!");
app.MapGet("/health", (ILogger<Program> logger) => logger.LogInformation("健康检查"));

app.Run();