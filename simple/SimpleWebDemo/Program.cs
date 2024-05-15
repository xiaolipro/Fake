using System.Text;
using Microsoft.AspNetCore.Routing.Internal;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseAutofac();
builder.Services.AddApplication<SimpleWebDemoModule>();
var app = builder.Build();
app.InitializeApplication();

app.UseStaticFiles();

byte[] plainTextPayload = Encoding.UTF8.GetBytes("Plain text!");

app.UseRouting();
app.Map("/getwithattributes", Handler);

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

app.MapControllers();

app.Run();

[Authorize]
Task HandlerWithAttributes(HttpContext context)
{
    return context.Response.WriteAsync("I have ann authorize attribute");
}

[HttpGet]
Task Handler(HttpContext context)
{
    return context.Response.WriteAsync("I have a method metadata attribute");
}

class AuthorizeAttribute : Attribute
{
}

class HttpGetAttribute : Attribute, IHttpMethodMetadata
{
    public bool AcceptCorsPreflight => false;

    public IReadOnlyList<string> HttpMethods { get; } = new List<string> { "GET" };
}