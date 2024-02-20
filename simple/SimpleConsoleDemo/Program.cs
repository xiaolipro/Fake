using System.Security.Claims;
using Fake;
using Fake.Auditing;
using Fake.DependencyInjection;
using Fake.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleConsoleDemo;

public class Program
{
    static async Task Main(string[] args)
    {
        using var application = FakeApplicationFactory.Create<SimpleConsoleDemoModule>(options =>
        {
            options.Configuration.CommandLineArgs = args;
            options.UseAutofac();
        });
        application.InitializeApplication();
        //logger.LogInformation("Initializing the application... OK");

        var myAuditedObject1 = application.ServiceProvider.GetRequiredService<MyAuditedObject1>();

        var currentPrincipalAccessor = application.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
        currentPrincipalAccessor.Change(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "10086"),
            new Claim(ClaimTypes.Name, "faker"),
        })));

        var manager = application.ServiceProvider.GetRequiredService<IAuditingManager>();
        using (var scope = manager.BeginScope())
        {
            await myAuditedObject1.DoItAsync(new InputObject { Value1 = "forty-two", Value2 = 42 });
            await F(myAuditedObject1);
            await scope.SaveAsync();
        }
    }

    static async Task F(MyAuditedObject1 myAuditedObject1)
    {
        await myAuditedObject1.DoItAsync(new InputObject { Value1 = "asdasd", Value2 = 1 });
    }
}

public interface IMyAuditedObject : ITransientDependency
{
}

[Audited]
public class MyAuditedObject1 : IMyAuditedObject
{
    public virtual async Task<ResultObject> DoItAsync(InputObject inputObject)
    {
        await Task.Delay(1000);
        return new ResultObject
        {
            Value1 = inputObject.Value1 + "-result",
            Value2 = inputObject.Value2 + 1
        };
    }
}

public class ResultObject
{
    public string Value1 { get; set; }

    public int Value2 { get; set; }
}

public class InputObject
{
    public string Value1 { get; set; }

    public int Value2 { get; set; }
}