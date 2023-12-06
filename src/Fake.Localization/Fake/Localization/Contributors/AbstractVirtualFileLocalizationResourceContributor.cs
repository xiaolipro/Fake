using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;

namespace Fake.Localization.Contributors;

public abstract class AbstractVirtualFileLocalizationResourceContributor : ILocalizationResourceContributor
{
    private readonly string _virtualPath;
    private IVirtualFileProvider _virtualFileProvider;

    // culture : localized string container
    private volatile Dictionary<string?, ILocalizedStringContainer?> _localizedStringContainers;
    private bool _subscribedForChanges;
    private readonly object _syncObj = new();

    public AbstractVirtualFileLocalizationResourceContributor(string virtualPath)
    {
        _virtualPath = virtualPath;
    }

    public virtual void Initialize(LocalizationResourceInitializationContext context)
    {
        _virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();
    }

    public virtual LocalizedString? GetOrNull(string? cultureName, string name)
    {
        return GetLocalizedStringContainers().GetOrDefault(cultureName)?.GetLocalizedStringOrDefault(name);
    }

    public virtual void Fill(string? cultureName, Dictionary<string, LocalizedString?> dictionary)
    {
        var localizedStringContainer = GetLocalizedStringContainers().GetOrDefault(cultureName);

        localizedStringContainer?.Fill(dictionary);
    }

    public virtual Task FillAsync(string? cultureName, Dictionary<string, LocalizedString?> dictionary)
    {
        var localizedStringContainer = GetLocalizedStringContainers().GetOrDefault(cultureName);

        localizedStringContainer?.Fill(dictionary);

        return Task.CompletedTask;
    }

    public virtual Task<IEnumerable<string?>> GetSupportedCulturesAsync()
    {
        var cultures = GetLocalizedStringContainers().Keys;

        return Task.FromResult<IEnumerable<string>>(cultures);
    }

    private Dictionary<string?, ILocalizedStringContainer?> GetLocalizedStringContainers()
    {
        if (_localizedStringContainers != null) return _localizedStringContainers;

        lock (_syncObj)
        {
            if (_localizedStringContainers != null) return _localizedStringContainers;

            _localizedStringContainers = CreateLocalizedStringContainers();
        }

        if (_subscribedForChanges) return _localizedStringContainers;

        var filter = _virtualPath.EndsWithOrAppend("/") + "*.*";

        // 订阅change事件
        void ChangeTokenConsumer()
        {
            _localizedStringContainers = null;
        }

        ChangeToken.OnChange(() => _virtualFileProvider.Watch(filter), ChangeTokenConsumer);

        _subscribedForChanges = true;

        return _localizedStringContainers;
    }


    private Dictionary<string?, ILocalizedStringContainer?> CreateLocalizedStringContainers()
    {
        var localizedStringContainers = new Dictionary<string?, ILocalizedStringContainer?>();

        foreach (var file in _virtualFileProvider.GetDirectoryContents(_virtualPath))
        {
            if (file.IsDirectory) continue;

            if (!CanParse(file)) continue;

            ILocalizedStringContainer? container;
            var path = file.GetVirtualOrPhysicalPathOrNull();
            using (var stream = file.CreateReadStream())
            {
                var fileContent = stream.ReadAsUTF8String();
                container = CreateLocalizedStringContainer(fileContent, path);
            }

            if (localizedStringContainers.ContainsKey(container.CultureName))
            {
                throw new FakeException($"已经存在Culture为：{container.CultureName}的本地化文件 {container.Path}");
            }

            localizedStringContainers[container.CultureName] = container;
        }

        return localizedStringContainers;
    }

    /// <summary>
    /// 文件是否可解析
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    protected abstract bool CanParse(IFileInfo file);

    /// <summary>
    /// 创建本地化字符串容器
    /// </summary>
    /// <param name="content"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    protected abstract ILocalizedStringContainer? CreateLocalizedStringContainer(string content, string path);
}