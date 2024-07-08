using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fake.Data;
using Fake.Data.Filtering;
using Fake.DependencyInjection;
using Fake.IdGenerators;
using Fake.IdGenerators.GuidGenerator;
using Fake.IdGenerators.Snowflake;
using Fake.Json;
using Fake.Json.SystemTextJson;
using Fake.Json.SystemTextJson.Converters;
using Fake.Json.SystemTextJson.Modifiers;
using Fake.Modularity;
using Fake.Threading;
using Fake.Timing;

/// <summary>
/// 核心模块
/// <remarks>自动载入，无须依赖</remarks>
/// </summary>
public class FakeCoreModule : FakeModule
{
    public override bool SkipServiceRegistration => true;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(IAmbientScopeProvider<>), typeof(AmbientScopeProvider<>));
        context.Services.AddTransient<ICancellationTokenProvider, NullCancellationTokenProvider>();
        context.Services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();

        ConfigureClock(context);

        ConfigureSystemTextJson(context);

        ConfigureIdGenerator(context);

        ConfigureData(context);
    }

    private void ConfigureData(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IDataFilter, DataFilter>();
        context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        context.Services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
    }

    private void ConfigureClock(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IFakeClock, Clock>();
        context.Services.Configure<FakeClockOptions>(_ => { });
    }

    private void ConfigureSystemTextJson(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IFakeJsonSerializer, FakeSystemTextJsonSerializer>();
        context.Services.AddTransient<DateTimeConverter>();
        context.Services.AddTransient<BooleanConverter>();
        context.Services.AddTransient<LongConverter>();
        context.Services.AddTransient<FakeDefaultJsonTypeInfoResolver>();
        context.Services.AddOptions<FakeSystemTextJsonModifiersOptions>()
            .Configure<IServiceProvider>((option, provider) =>
            {
                option.Modifiers.Add(new FakeDateTimeConverterModifier().CreateModifyAction(provider));
            });
        context.Services.AddOptions<JsonSerializerOptions>()
            .Configure<IServiceProvider>((options, provider) =>
            {
                // If the user hasn't explicitly configured the encoder,
                // use the less strict encoder that does not encode all non-ASCII characters.
                // tip: 宽松模式，不对非ASCII字符进行编码
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                // Web环境默认配置
                options.PropertyNameCaseInsensitive = true;
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.NumberHandling = JsonNumberHandling.AllowReadingFromString;

                // Fake默认配置
                options.ReadCommentHandling = JsonCommentHandling.Skip;
                options.AllowTrailingCommas = true;

                options.Converters.Add(provider.GetRequiredService<DateTimeConverter>());
                options.Converters.Add(provider.GetRequiredService<LongConverter>());
                options.Converters.Add(provider.GetRequiredService<BooleanConverter>());
                options.Converters.Add(new ObjectToInferredTypesConverter());

                options.TypeInfoResolver = provider.GetRequiredService<FakeDefaultJsonTypeInfoResolver>();
            });
    }

    private static void ConfigureIdGenerator(ServiceConfigurationContext context)
    {
        // 请注意数据库适配问题
        context.Services.AddSingleton(typeof(IIdGenerator<Guid>),
            new SequentialGuidGenerator(SequentialGuidType.SequentialAsString));

        // 雪花Id
        context.Services.AddSingleton<SnowflakeIdGenerator>();
        context.Services.AddSingleton<IIdGenerator<long>, SnowflakeIdGenerator>();
        context.Services.AddSingleton<IWorkerProvider, DefaultWorkerProvider>();
        context.Services.Configure<SnowflakeIdGeneratorOptions>(_ => { });
    }
}