using Fake.Data.Filtering;
using Fake.DependencyInjection;
using Fake.Localization;
using Fake.ObjectMapping;
using Fake.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Fake.DomainDrivenDesign.Application;

public abstract class ApplicationService : IRemoteService, ITransientDependency
{
    // 属性注入，必须public
    public ILazyServiceProvider LazyServiceProvider { get; set; } = default!;

    protected ILogger Logger => LazyServiceProvider.GetService<ILogger>(provider =>
        provider.GetRequiredService<ILoggerFactory>().CreateLogger(GetType().FullName ?? string.Empty))!;

    protected IObjectMapper ObjectMapper => LazyServiceProvider.GetRequiredService<IObjectMapper>();

    protected ICurrentUser CurrentUser => LazyServiceProvider.GetRequiredService<ICurrentUser>();

    protected IDataFilter DataFilter => LazyServiceProvider.GetRequiredService<IDataFilter>();

    private IStringLocalizer? _localizer;
    protected IStringLocalizer L => _localizer ??= CreateLocalizer();

    // 若不设定值则使用FakeLocalizationOptions.DefaultResourceType，若仍然无值则无法使用L
    protected Type? LocalizationResource = null;

    protected virtual IStringLocalizer CreateLocalizer()
    {
        var stringLocalizerFactory = LazyServiceProvider.GetRequiredService<IStringLocalizerFactory>();

        if (LocalizationResource != null)
        {
            return stringLocalizerFactory.Create(LocalizationResource);
        }

        var localizer = stringLocalizerFactory.CreateDefaultOrNull();

        if (localizer == null)
        {
            throw new FakeException(
                $"设置 {nameof(LocalizationResource)} 或配置 {nameof(FakeLocalizationOptions)}.{nameof(FakeLocalizationOptions.DefaultResourceType)}) 后才能使用 {nameof(L)} !");
        }

        return localizer;
    }
}