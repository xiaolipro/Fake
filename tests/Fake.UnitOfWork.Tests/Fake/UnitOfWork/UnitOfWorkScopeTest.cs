using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Fake.UnitOfWork;

public class UnitOfWorkScopeTest : ApplicationTest<FakeUnitOfWorkModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkScopeTest()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    async Task 嵌套默认共享外层工作单元()
    {
        _unitOfWorkManager.Current.ShouldBeNull();

        using (var uow1 = _unitOfWorkManager.Begin())
        {
            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            using (var uow2 = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Current.ShouldNotBeNull();
                _unitOfWorkManager.Current.Id.ShouldBe(uow1.Id);

                using (var uow3 = _unitOfWorkManager.Begin(new UnitOfWorkAttribute()
                       {
                           RequiresNew = true
                       }))
                {
                    _unitOfWorkManager.Current.ShouldNotBeNull();
                    _unitOfWorkManager.Current.Id.ShouldNotBe(uow1.Id);

                    await uow3.CompleteAsync();
                }

                _unitOfWorkManager.Current.ShouldNotBeNull();
                _unitOfWorkManager.Current.ShouldBe(uow1);

                await uow2.CompleteAsync();
            }

            _unitOfWorkManager.Current.ShouldNotBeNull();
            _unitOfWorkManager.Current.ShouldBe(uow1);

            await uow1.CompleteAsync();
        }

        _unitOfWorkManager.Current.ShouldBeNull();
    }
}