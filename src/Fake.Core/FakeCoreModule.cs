using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fake.DependencyInjection;
using Fake.Json;
using Fake.Json.Converters;
using Fake.Modularity;
using Fake.Threading;
using Fake.Timing;

/// <summary>
/// 核心模块
/// <remarks>自动载入，无须依赖</remarks>
/// </summary>
public class FakeCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IFakeClock, FakeClock>();
        context.Services.AddTransient(typeof(IAmbientScopeProvider<>), typeof(AmbientScopeProvider<>));
        context.Services.AddTransient<ICancellationTokenProvider, NullCancellationTokenProvider>();

        context.Services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();

        // Json序列化
        context.Services.AddTransient<IFakeJsonSerializer, FakeSystemTextJsonSerializer>();
        context.Services.AddTransient<FakeDateTimeConverter>();
        context.Services.AddTransient<FakeBooleanConverter>();
        context.Services.AddTransient<FakeLongConverter>();
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
            });
    }
}