using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Fake.UnitOfWork;

public class UnitOfWorkActionTest : ApplicationTest<FakeUnitOfWorkModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkActionTest()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    async Task Complete时()
    {
        var completed = false;
        var disposed = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.OnCompleted(() =>
            {
                completed = true;
                return Task.CompletedTask;
            });

            uow.Disposed += (sender, args) => disposed = true;

            await uow.CompleteAsync();

            completed.ShouldBeTrue();
        }

        disposed.ShouldBeTrue();
    }

    [Fact]
    void 异常会触发Failed事件()
    {
        var completed = false;
        var failed = false;
        var disposed = false;

        Assert.Throws<Exception>(new Action(() =>
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                uow.OnCompleted(() =>
                {
                    completed = true;
                    return Task.CompletedTask;
                });
                uow.Failed += (sender, args) => failed = true;
                uow.Disposed += (sender, args) => disposed = true;
                throw new Exception("test exception");
            }
        })).Message.ShouldBe("test exception");

        completed.ShouldBeFalse();
        failed.ShouldBeTrue();
        disposed.ShouldBeTrue();
    }

    [InlineData(true)]
    [InlineData(false)]
    [Theory]
    public async Task 回滚会触发Failed事件(bool callComplete)
    {
        var completed = false;
        var failed = false;
        var disposed = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.OnCompleted(() =>
            {
                completed = true;
                return Task.CompletedTask;
            });

            uow.Failed += (sender, args) =>
            {
                failed = true;
                args.IsRollBacked.ShouldBeTrue();
            };
            uow.Disposed += (sender, args) => disposed = true;

            await uow.RollbackAsync();

            if (callComplete)
            {
                await uow.CompleteAsync();
            }
        }

        completed.ShouldBeFalse();
        failed.ShouldBeTrue();
        disposed.ShouldBeTrue();
    }

    [Fact]
    void Dispose时()
    {
        bool dispose = false;
        using (var uow = _unitOfWorkManager.Begin())
        {
            uow.Disposed += (sender, args) => { dispose = true; };
        }

        dispose.ShouldBe(true);
    }


    [Fact]
    async Task 子工作单元提交行为不产生效果()
    {
        var completed = false;
        var disposed = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            using (var childUow = _unitOfWorkManager.Begin())
            {
                childUow.OnCompleted(() =>
                {
                    completed = true;
                    return Task.CompletedTask;
                });

                uow.Disposed += (sender, args) => disposed = true;

                await childUow.CompleteAsync();

                completed.ShouldBeFalse(); //Parent has not been completed yet!
                disposed.ShouldBeFalse();
            }

            completed.ShouldBeFalse(); //Parent has not been completed yet!
            disposed.ShouldBeFalse();

            await uow.CompleteAsync();

            completed.ShouldBeTrue(); //It's completed now!
            disposed.ShouldBeFalse(); //But not disposed yet!
        }

        disposed.ShouldBeTrue();
    }
}