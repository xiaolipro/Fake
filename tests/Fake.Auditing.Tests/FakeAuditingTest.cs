using Fake;
using Fake.Testing;

public class FakeAuditingTest : FakeIntegrationTest<FakeAuditingTestModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}