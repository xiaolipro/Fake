using Fake;
using Fake.Modularity;
using Fake.Testing;

public abstract class AppTestBase<TStartupModule>:FakeIntegrationTest<TStartupModule>
    where TStartupModule: IFakeModule
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}