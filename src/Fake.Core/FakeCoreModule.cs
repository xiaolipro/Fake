using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fake.DependencyInjection;
using Fake.IDGenerators;
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
    public override bool IsFakeFrameworkModule => true;
    public override bool SkipAutoServiceRegistration => true;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(IAmbientScopeProvider<>), typeof(AmbientScopeProvider<>));
        context.Services.AddTransient<ICancellationTokenProvider, NullCancellationTokenProvider>();
        context.Services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();
        
        ConfigureClock(context);

        ConfigureSystemTextJson(context);

        ConfigureIdGenerator(context);
    }

    private static void ConfigureClock(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IFakeClock, FakeClock>();
        context.Services.Configure<FakeClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Unspecified;
            options.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        });
    }

    private static void ConfigureSystemTextJson(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IFakeJsonSerializer, FakeSystemTextJsonSerializer>();
        context.Services.AddTransient<FakeDateTimeConverter>();
        context.Services.AddTransient<FakeBooleanConverter>();
        context.Services.AddTransient<FakeLongConverter>();
        context.Services.AddTransient<FakeDefaultJsonTypeInfoResolver>();
        context.Services.AddOptions<FakeSystemTextJsonModifiersOption>()
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

                options.Converters.Add(provider.GetRequiredService<FakeDateTimeConverter>());
                options.Converters.Add(provider.GetRequiredService<FakeLongConverter>());
                options.Converters.Add(provider.GetRequiredService<FakeBooleanConverter>());
                options.Converters.Add(new ObjectToInferredTypesConverter());

                options.TypeInfoResolver = provider.GetRequiredService<FakeDefaultJsonTypeInfoResolver>();
            });
    }

    private static void ConfigureIdGenerator(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IGuidGenerator, SequentialGuidGenerator>();
        // 请注意数据库适配问题
        context.Services.Configure<SequentialGuidGeneratorOptions>(options =>
        {
            // 默认生成的有序guid是SequentialAsBinaryAtEnd类型的（SQLSERVER友好的）
            options.SequentialGuidType = SequentialGuidType.SequentialAsString;
        });
    }
}