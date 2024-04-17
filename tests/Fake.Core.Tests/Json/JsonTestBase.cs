using Fake.Testing;

namespace Fake.Core.Tests.Json;

public abstract class JsonTestBase : ApplicationTest<FakeCoreModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}