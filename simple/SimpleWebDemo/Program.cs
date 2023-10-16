using SimpleWebDemo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFakeApplication<SimpleWebDemoModule>();
var app = builder.Build();
app.InitializeApplication();

var ebv = app.Services.GetService<IWebHostEnvironment>();
app.UseStaticFiles();
app.MapGet("/", () => "Hello World!");
app.Run();