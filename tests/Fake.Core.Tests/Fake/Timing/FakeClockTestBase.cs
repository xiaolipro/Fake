using Fake.Testing;

namespace Fake.Timing;

public class FakeClockTestBase : FakeIntegrationTest<FakeCoreModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options) => options.UseAutofac();
}