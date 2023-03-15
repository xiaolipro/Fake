using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;

namespace Fake.Localization.Contributors;

public abstract class AbstractVirtualFileLocalizationResourceContributor : ILocalizationResourceContributor
{
    private readonly string _virtualPath;
    public VirtualFileProvider VirtualFileProvider { get; set; }

    // culture : localized string container
    private volatile Dictionary<string, ILocalizedStringContainer> _localizedStringContainers;
    private bool _subscribedForChanges;
    private readonly object _syncObj = new();

    public AbstractVirtualFileLocalizationResourceContributor(string virtualPath)
    {
        _virtualPath = virtualPath;
    }

    public LocalizedString GetOrNull(string cultureName, string name)
    {
        return GetLocalizedStringContainers().GetOrDefault(cultureName)?.GetLocalizedStringOrDefault(name);
    }

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var localizedStringContainer = GetLocalizedStringContainers().GetOrDefault(cultureName);

        localizedStringContainer?.Fill(dictionary);
    }
    
    public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var localizedStringContainer = GetLocalizedStringContainers().GetOrDefault(cultureName);

        localizedStringContainer?.Fill(dictionary);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        var cultures = GetLocalizedStringContainers().Keys;

        return Task.FromResult<IEnumerable<string>>(cultures);
    }

    private Dictionary<string, ILocalizedStringContainer> GetLocalizedStringContainers()
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

        ChangeToken.OnChange(() => VirtualFileProvider.Watch(filter), ChangeTokenConsumer);

        _subscribedForChanges = true;

        return _localizedStringContainers;
    }


    private Dictionary<string, ILocalizedStringContainer> CreateLocalizedStringContainers()
    {
        var dic = new Dictionary<string, ILocalizedStringContainer>();

        foreach (var file in VirtualFileProvider.GetDirectoryContents(_virtualPath))
        {
            if (file.IsDirectory) continue;

            if (!CanParse(file)) continue;

            ILocalizedStringContainer container;
            var path = file.GetVirtualOrPhysicalPathOrNull();
            using (var stream = file.CreateReadStream())
            {
                var fileContent = stream.ReadAsUTF8String();
                container = CreateLocalizedStringContainer(fileContent, path);
            }

            if (_localizedStringContainers.ContainsKey(container.CultureName))
            {
                throw new FakeException($"已经存在Culture为：{container.CultureName}的本地化文件 {container.Path}");
            }
            _localizedStringContainers[container.CultureName] = container;
        }

        return dic;
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
    protected abstract ILocalizedStringContainer CreateLocalizedStringContainer(string content, string path);
}