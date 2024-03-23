using System;
using Fake.Data.Filtering;
using Fake.DependencyInjection;
using Fake.DomainDrivenDesign.Entities.Auditing;
using Fake.EventBus;
using Fake.IdGenerators;
using Fake.Timing;
using Fake.UnitOfWork;
using SqlSugar;

namespace Fake.SqlSugarCore;

public class SugarDbContext
{
    public ILazyServiceProvider ServiceProvider { get; set; } = null!;
    public ISqlSugarClient SqlSugarClient { get; private set; }

    protected IFakeClock FakeClock => ServiceProvider.GetRequiredService<IFakeClock>();
    protected GuidGeneratorBase GuidGenerator => ServiceProvider.GetRequiredService<GuidGeneratorBase>();
    protected LongIdGeneratorBase LongIdGenerator => ServiceProvider.GetRequiredService<LongIdGeneratorBase>();
    protected LocalEventBus LocalEventBus => ServiceProvider.GetRequiredService<LocalEventBus>();
    protected IAuditPropertySetter AuditPropertySetter => ServiceProvider.GetRequiredService<IAuditPropertySetter>();
    protected IDataFilter DataFilter => ServiceProvider.GetRequiredService<IDataFilter>();


    public virtual void Initialize(IUnitOfWork unitOfWork)
    {
        // 设置超时时间
        if (Database.IsRelational())
        {
            if (!Database.GetCommandTimeout().HasValue)
            {
                Database.SetCommandTimeout(TimeSpan.FromMilliseconds(unitOfWork.Context.Timeout));
            }
        }

        // 级联删除策略
        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;

        // 实体Track事件
        ChangeTracker.Tracked += (_, args) =>
        {
            // TODO：ExtraProperties Tracked?

            // 为跟踪实体发布事件
            PublishEventsForTrackedEntity(args.Entry);
        };

        // 实体状态变更事件
        ChangeTracker.StateChanged += (_, args) =>
        {
            // 为跟踪实体发布事件
            PublishEventsForTrackedEntity(args.Entry);
        };
    }
}