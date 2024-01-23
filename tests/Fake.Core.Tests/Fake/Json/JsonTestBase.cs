using Fake.Testing;

namespace Fake.Json;

public abstract class JsonTestBase : ApplicationTest<FakeCoreModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}