using System.Collections.Generic;
using System.Linq;
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
        var allVirtualFileProviders = new List<IFileProvider>();
        
        allVirtualFileProviders.Add(dynamicFileProvider);

        // Notes：
        //   这里翻转顺序非常关键，如果两个模块将文件添加到相同的虚拟路径(如my-path/my-file.css)
        // 之后添加的模块将替换前一个(模块依赖顺序决定了添加文件的顺序).
        // 实质上并非真正移除之前模块的虚拟文件，而是在寻找到第一个满足的文件时就离开寻找
        allVirtualFileProviders.AddRange(_options.FileProviders.AsEnumerable().Reverse());

        return new CompositeFileProvider(allVirtualFileProviders);
    }
}