using System.Collections.Generic;
using System.Threading.Tasks;
using Fake.VirtualFileSystem;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;

namespace Fake.Localization.VirtualFile;

public abstract class AbstractVirtualFileLocalizationResourceContributor: ILocalizationResourceContributor
{
    private readonly string _virtualPath;
    private VirtualFileProvider _virtualFileProvider;
    
    // culture:localized string info
    private Dictionary<string, ILocalizedStringContainer> _localizedStringContainers;
    private bool _subscribedForChanges;
    private AbstractLocalizationResource _localizationResource;
    private readonly object _syncObj = new object();

    public AbstractVirtualFileLocalizationResourceContributor(string virtualPath)
    {
        _virtualPath = virtualPath;
    }
    
    public void Initialize(LocalizationResourceInitializationContext context)
    {
        throw new System.NotImplementedException();
    }

    public LocalizedString GetOrNull(string cultureName, string name)
    {
        throw new System.NotImplementedException();
    }

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        throw new System.NotImplementedException();
    }

    public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        throw new System.NotImplementedException();
    }


    private Dictionary<string, ILocalizedStringContainer> CreateLocalizedStringInfoDictionary()
    {
        var dic = new Dictionary<string, ILocalizedStringContainer>();

        foreach (var file in _virtualFileProvider.GetDirectoryContents(_virtualPath))
        {
            if (file.IsDirectory) continue;
            
            if (!CanParse(file)) continue;

            ILocalizedStringContainer container;
            using (var stream = file.CreateReadStream())
            {
                container = CreateLocalizedStringContainer(Utf8Helper.ReadStringFromStream(stream));
            }

            if (_localizedStringContainers.ContainsKey(container.CultureName))
            {
                throw new FakeException($"{file.GetVirtualOrPhysicalPathOrNull()} ")
            }
        }
    }

    protected abstract bool CanParse(IFileInfo file);

    protected abstract ILocalizedStringContainer CreateLocalizedStringContainer(string fileContent);


    protected virtual ILocalizedStringContainer CreateLocalizedStringContainer(IFileInfo file)
    {
        using (var stream = file.CreateReadStream())
        {
            
        }
    }
}