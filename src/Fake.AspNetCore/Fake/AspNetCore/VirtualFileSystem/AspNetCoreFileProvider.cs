using System.Collections.Generic;
using Fake.VirtualFileSystem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Fake.AspNetCore.VirtualFileSystem;

/// <summary>
/// AspNetCore文件供应商，由Fake虚拟文件系统和wwwroot文件系统组合而成
/// </summary>
public class AspNetCoreFileProvider:IAspNetCoreFileProvider
{
    protected FakeAspNetCoreFileOptions FileOptions { get; }
    private readonly IFileProvider _webRootFileProvider;

    public AspNetCoreFileProvider(IVirtualFileProvider virtualFileProvider,
        IWebHostEnvironment hostingEnvironment,
        IOptions<FakeAspNetCoreFileOptions> options)
    {
        FileOptions = options.Value;
        _webRootFileProvider = CreateWebRootFileProvider(virtualFileProvider, hostingEnvironment);
    }

    private IFileProvider CreateWebRootFileProvider(IVirtualFileProvider virtualFileProvider,
        IWebHostEnvironment webHostEnvironment)
    {
        // 将虚拟文件系统和wwwroot物理文件系统合并
        var fileProviders = new List<IFileProvider>
        {
            new PhysicalFileProvider(webHostEnvironment.ContentRootPath),
            virtualFileProvider
        };

        return new CompositeFileProvider(
            fileProviders
        );
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        throw new System.NotImplementedException();
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        throw new System.NotImplementedException();
    }

    public IChangeToken Watch(string filter)
    {
        throw new System.NotImplementedException();
    }
}