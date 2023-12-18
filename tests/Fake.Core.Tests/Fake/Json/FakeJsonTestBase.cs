using Fake.Testing;

namespace Fake.Json;

public abstract class FakeJsonTestBase : FakeApplicationTest<FakeCoreModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}