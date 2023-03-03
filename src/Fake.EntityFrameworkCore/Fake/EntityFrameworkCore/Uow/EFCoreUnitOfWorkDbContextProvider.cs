using System.Threading.Tasks;
using Fake.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Fake.EntityFrameworkCore.Uow;

public class EFCoreUnitOfWorkDbContextProvider<TDbContext> : IEFCoreDbContextProvider<TDbContext>
    where TDbContext : IFakeDbContext
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly EFCoreDbContextOptions _options;

    public EFCoreUnitOfWorkDbContextProvider(IUnitOfWorkManager unitOfWorkManager,
        IOptions<EFCoreDbContextOptions> options)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _options = options.Value;
    }
    public Task<IUnitOfWork> GetDbContextAsync()
    {
        var uow = _unitOfWorkManager.Current;
        
        if (uow is null)
        {
            throw new FakeException("UnitOfWorkDbContext必须在工作单元内工作！");
        }

        return Task.FromResult(uow);
    }
}