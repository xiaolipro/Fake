using Fake;
using Fake.Modularity;
using Fake.Testing;
using Fake.UnitOfWork;

public abstract class AppTestBase<TStartupModule> : ApplicationTestWithTools<TStartupModule>
    where TStartupModule : IFakeModule
{
    protected override void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }


    protected virtual Task WithUnitOfWorkAsync(Func<Task> func)
    {
        return WithUnitOfWorkAsync(new UnitOfWorkAttribute(), func);
    }

    protected virtual async Task WithUnitOfWorkAsync(UnitOfWorkAttribute options, Func<Task> action)
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            using (var uow = uowManager.Begin(options))
            {
                await action();
                await uow.CompleteAsync();
            }
        }
    }
}