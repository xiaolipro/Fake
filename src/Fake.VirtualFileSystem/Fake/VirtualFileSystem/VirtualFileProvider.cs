using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Fake.VirtualFileSystem;

public class VirtualFileProvider: IFileProvider
{
    private readonly FakeVirtualFileSystemOptions _options;
    private readonly CompositeFileProvider _compositeFileProvider;

    public VirtualFileProvider(IOptions<FakeVirtualFileSystemOptions> options,DynamicFileProvider dynamicFileProvider)
    {
        _options = options.Value;
        _compositeFileProvider = CreateCompositeFileProvider(dynamicFileProvider);
    }
    public IFileInfo GetFileInfo(string subpath)
    {
        return _compositeFileProvider.GetFileInfo(subpath);
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return _compositeFileProvider.GetDirectoryContents(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return _compositeFileProvider.Watch(filter);
    }


    private CompositeFileProvider CreateCompositeFileProvider(DynamicFileProvider dynamicFileProvider)
    {
        var fileProviders = new List<IFileProvider>();
        
        fileProviders.Add(dynamicFileProvider);

        foreach (var fileSet in _options.FileSets.FileProviders)
        {
            fileProviders.Add(fileSet);
        }

        return new CompositeFileProvider(fileProviders);
    }
}