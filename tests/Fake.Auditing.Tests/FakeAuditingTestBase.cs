using Fake;
using Fake.Testing;

public class FakeAuditingTestBase : FakeModuleTest<FakeAuditingTestModuleApplication>
{
    protected override void SetFakeApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}