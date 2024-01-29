using System.Net;
using System.Net.Quic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFakeApplication<SimpleWebDemoModule>();
var app = builder.Build();
app.InitializeApplication();

var log = app.Services.GetRequiredService<ILogger<Program>>();
if (QuicConnection.IsSupported)
{
    using var client = new HttpClient
    {
        DefaultRequestVersion = HttpVersion.Version30,
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact
    };

    HttpResponseMessage resp = await client.GetAsync("https://cloudflare-quic.com/");
    string body = await resp.Content.ReadAsStringAsync();

    Console.WriteLine(
        $"status: {resp.StatusCode}, version: {resp.Version}, " +
        $"body: {body.Substring(0, Math.Min(100, body.Length))}");
}
else
{
    log.LogInformation("QUIC is not supported.");
}

app.UseStaticFiles();
app.MapGet("/", () => "Hello World!");
app.Run();