using System;
using System.Threading.Tasks;
using Fake;
using Fake.Auditing;
using Fake.DependencyInjection;
using Fake.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleConsoleDemo;

public class Program
{
    static async Task Main(string[] args)
    {
        using (var application = FakeApplicationFactory.Create<SimpleConsoleDemoModule>(options =>
               {
                   options.Configuration.CommandLineArgs = args;
                   options.UseAutofac();
                   options.Services.AddSingleton(typeof(IScopeProvider<>), typeof(ScopeProvider<>));
               }))
        {
            Console.WriteLine("Initializing the application...");
            application.Initialize();
            Console.WriteLine("Initializing the application... OK");
            
            var myAuditedObject1 = application.ServiceProvider.GetRequiredService<MyAuditedObject1>();
            
            using (var scope = application.ServiceProvider.GetRequiredService<IAuditingManager>().BeginScope())
            {
                await myAuditedObject1.DoItAsync(new InputObject { Value1 = "forty-two", Value2 = 42 });
            }
        }
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