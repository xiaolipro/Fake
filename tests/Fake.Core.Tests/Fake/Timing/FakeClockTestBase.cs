using Fake.Testing;

namespace Fake.Timing;

public class FakeClockTestBase : FakeApplicationTest<FakeCoreModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options) =>
        options.UseAutofac();
}