using System.Text;
using Microsoft.AspNetCore.Routing.Internal;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseAutofac().UseSerilog();
;
builder.Services.AddApplication<SimpleWebDemoModule>();
var app = builder.Build();
app.InitializeApplication();

byte[] plainTextPayload = "Plain text!"u8.ToArray();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/hello1", (string name) => $"Hello World! {name}");
    endpoints.MapGet("/hello6", (string name) => $"Hello World! {name}");

    endpoints.MapGet(
        "/",
        (httpContext) =>
        {
            var dataSource = httpContext.RequestServices.GetRequiredService<EndpointDataSource>();

            var sb = new StringBuilder();
            sb.AppendLine("Endpoints:");
            foreach (var endpoint in dataSource.Endpoints.OfType<RouteEndpoint>()
                         .OrderBy(e => e.RoutePattern.RawText, StringComparer.OrdinalIgnoreCase))
            {
                sb.AppendLine(FormattableString.Invariant($"- {endpoint.RoutePattern.RawText}"));
                foreach (var metadata in endpoint.Metadata)
                {
                    sb.AppendLine("    " + metadata);
                }
            }

            var response = httpContext.Response;
            response.StatusCode = 200;
            response.ContentType = "text/plain";
            return response.WriteAsync(sb.ToString());
        });
    endpoints.MapGet(
        "/plaintext",
        (httpContext) =>
        {
            var response = httpContext.Response;
            var payloadLength = plainTextPayload.Length;
            response.StatusCode = 200;
            response.ContentType = "text/plain";
            response.ContentLength = payloadLength;
            return response.Body.WriteAsync(plainTextPayload, 0, payloadLength);
        });
    endpoints.MapGet(
        "/graph",
        (httpContext) =>
        {
            using (var writer = new StreamWriter(Console.OpenStandardOutput(), Encoding.UTF8, 1024, leaveOpen: true))
            {
                var graphWriter = httpContext.RequestServices.GetRequiredService<DfaGraphWriter>();
                var dataSource = httpContext.RequestServices.GetRequiredService<EndpointDataSource>();
                graphWriter.Write(dataSource, writer);
            }

            return Task.CompletedTask;
        }).WithDisplayName("DFA Graph");

    endpoints.MapGet("/attributes", (string name) => $"Hello World! {name}");
});

app.Run();