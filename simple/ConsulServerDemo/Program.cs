using ConsulServerDemo;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfigurationFromConsul();
builder.Services.AddApplication<ConsulServerDemoModule>();
var app = builder.Build();
app.InitializeApplication();
app.MapGet("/say", () => "Hello World!");

app.MapGet("/config", () => builder.Configuration["name"]);

app.MapGet("/config2", (IOptions<Student> options) => options.Value.Name + "" + options.Value.Age);
app.MapGet("/config3", (IOptionsSnapshot<Student> options) => options.Value.Name + "" + options.Value.Age);

app.Run();


public class Student
{
    public string? Name { get; set; }

    public int Age { get; set; }
}