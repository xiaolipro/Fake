using Fake;
using Fake.Authorization.Tests;
using Fake.Testing;

public class AuthorizationTestBase : FakeIntegrationTest<FakeAuthorizationTestModule>
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        base.SetApplicationCreationOptions(options);
        options.UseAutofac();
    }
}