using System;
using System.Collections.Generic;
using Fake.Http.Fake.Http;
using Fake.Json;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fake.Http
{
    public class FakeHttpModule : FakeModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddFakeHttp(setup =>
            {
                setup.BaseAddress = "http://localhost:8080/";
                setup.Accept = new[]
                {
                    "application/json",
                    "text/plain",
                    "*/*"
                };
                setup.EnableCompress = false;
                setup.Headers = new[]{
                    new KeyValuePair<string, string> ("X-Ca-Test", "key")
                };
            });
            context.Services.AddSingleton<IFakeHttp, FakeHttp>();
            base.ConfigureServices(context);
        }

        public override void ConfigureApplication(ApplicationConfigureContext context)
        {
            FakeHttpLocator.UseFakeHttp(
                context.ServiceProvider.GetRequiredService<IFakeJsonSerializer>(),
                context.ServiceProvider.GetRequiredService<ILogger<FakeHttp>>());


            base.ConfigureApplication(context);
        }
    }
}
