using SimpleWebDemo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFakeApplication<SimpleWebDemoModule>();
var app = builder.Build();
app.InitializeApplication();

app.MapGet("/", () => "Hello World!");

app.Run();