using System.Reflection;
using System.Text;
using Fake.Data.Filtering;
using Fake.DependencyInjection;
using Fake.DomainDrivenDesign.Entities.Auditing;
using Fake.DomainDrivenDesign.Events;
using Fake.EventBus;
using Fake.Helpers;
using Fake.IdGenerators;
using Fake.Timing;
using Fake.UnitOfWork;

namespace Fake.SqlSugarCore;

public class SugarDbContext
{
    public ILazyServiceProvider ServiceProvider { get; set; } = null!;
    public ISqlSugarClient SqlSugarClient { get; private set; } = null!;

    protected IFakeClock FakeClock => ServiceProvider.GetRequiredService<IFakeClock>();
    protected GuidGeneratorBase GuidGenerator => ServiceProvider.GetRequiredService<GuidGeneratorBase>();
    protected LongIdGeneratorBase LongIdGenerator => ServiceProvider.GetRequiredService<LongIdGeneratorBase>();
    protected LocalEventBus LocalEventBus => ServiceProvider.GetRequiredService<LocalEventBus>();
    protected IAuditPropertySetter AuditPropertySetter => ServiceProvider.GetRequiredService<IAuditPropertySetter>();
    protected IDataFilter DataFilter => ServiceProvider.GetRequiredService<IDataFilter>();
    protected ILogger<SugarDbContext> Logger => ServiceProvider.GetRequiredService<ILogger<SugarDbContext>>();
    protected SugarDbConnOptions Options => ServiceProvider.GetRequiredService<IOptions<SugarDbConnOptions>>().Value;

    public virtual void Initialize(IUnitOfWork unitOfWork)
    {
        // 设置超时时间
        if (SqlSugarClient.Ado.CommandTimeOut < 1)
        {
            SqlSugarClient.Ado.CommandTimeOut = unitOfWork.Context.Timeout;
        }

        SqlSugarClient.Aop.DataExecuting = DataExecuting;
        SqlSugarClient.Aop.DataExecuted = DataExecuted;
        SqlSugarClient.Aop.OnLogExecuting = OnLogExecuting;
        SqlSugarClient.Aop.OnLogExecuted = OnLogExecuted;

        ConfigureGlobalFilters();

        var config = new ConnectionConfig
        {
            ConfigId = Options.ConfigId,
            DbType = Options.DbType,
            ConnectionString = Options.ConnectionString,
            SlaveConnectionConfigs = Options.ReadConnectionStrings
                .Select(str => new SlaveConnectionConfig { ConnectionString = str }).ToList(),
            IsAutoCloseConnection = Options.IsAutoCloseConnection,
            AopEvents = new AopEvents
            {
                OnDiffLogEvent = null,
                OnError = null,
                OnLogExecuting = OnLogExecuting,
                OnLogExecuted = OnLogExecuted,
                OnExecutingChangeSql = null,
                DataExecuting = DataExecuting,
                DataExecuted = DataExecuted
            },
            ConfigureExternalServices = ConfigureExternalServices(),
            IndexSuffix = null,
            SqlMiddle = null
        };

        ConfigureConnection(config);

        SqlSugarClient = new SqlSugarClient(config);
    }

    protected virtual void ConfigureGlobalFilters()
    {
        //需自定义扩展
        if (DataFilter.IsEnabled<ISoftDelete>())
        {
            SqlSugarClient.QueryFilter.AddTableFilter<ISoftDelete>(u => u.IsDeleted == false);
        }
    }

    protected virtual void ConfigureConnection(ConnectionConfig action)
    {
    }

    protected virtual ConfigureExternalServices ConfigureExternalServices()
    {
        return new ConfigureExternalServices
        {
            EntityService = (property, column) =>
            {
                if (new NullabilityInfoContext().Create(property).WriteState is NullabilityState.Nullable)
                {
                    column.IsNullable = true;
                }

                if (property.Name == nameof(IHasDomainEvent.DomainEvents))
                {
                    column.IsIgnore = true;
                }

                if (property.Name == nameof(IHasExtraProperties.ExtraProperties))
                {
                    column.IsIgnore = true;
                }

                if (property.Name == nameof(Entity<object>.Id))
                {
                    column.IsPrimarykey = true;
                }
            }
        };
    }

    protected virtual async Task PublishDomainEventsAsync(IHasDomainEvent entityWithDomainEvent)
    {
        if (entityWithDomainEvent.DomainEvents == null) return;
        foreach (var @event in entityWithDomainEvent.DomainEvents)
        {
            await LocalEventBus.PublishAsync(@event);
        }
    }

    protected virtual void ConfigureBaseProperties(PropertyInfo property, EntityColumnInfo column)
    {
        if (property.PropertyType == typeof(ExtraPropertyDictionary))
        {
            column.IsIgnore = true;
        }

        if (property.Name == nameof(Entity<Any>.Id))
        {
            column.IsPrimarykey = true;
        }
    }


    #region DataExecuting

    protected virtual void DataExecuting(object oldValue, DataFilterModel entityInfo)
    {
        switch (entityInfo.OperationType)
        {
            case DataFilterType.UpdateByObject:
                AuditPropertySetter.SetModificationProperties(entityInfo.EntityValue);
                break;
            case DataFilterType.InsertByObject:
                AuditPropertySetter.SetCreationProperties(entityInfo.EntityValue);
                break;
            case DataFilterType.DeleteByObject:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected virtual void DataExecuted(object oldValue, DataAfterModel entityInfo)
    {
        if (oldValue is IHasDomainEvent entity)
        {
            AsyncHelper.RunSync(PublishDomainEventsAsync(entity));
        }
    }

    #endregion

    #region 日志

    protected virtual void OnLogExecuting(string sql, SugarParameter[] pars)
    {
        if (Options.EnabledSqlLog)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("SQL SUGAR LOG:");
            sb.AppendLine(UtilMethods.GetSqlString(DbType.SqlServer, sql, pars));
            Logger.LogDebug(sb.ToString());
        }
    }

    protected virtual void OnLogExecuted(string sql, SugarParameter[] pars)
    {
        if (Options.EnabledSqlLog)
        {
            Logger.LogDebug("- ExecutionDuration: {times} ms", SqlSugarClient.Ado.SqlExecutionTime.TotalMilliseconds);
        }
    }

    #endregion
}