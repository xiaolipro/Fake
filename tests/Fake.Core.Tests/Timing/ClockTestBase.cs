using Fake.Testing;

namespace Fake.Core.Tests.Timing;

public class ClockTestBase : ApplicationTest<FakeCoreModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options) =>
        options.UseAutofac();
}