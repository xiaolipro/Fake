using System.Collections.Generic;
using System.Reflection;
using Fake.Modularity;
using Fake.VirtualFileSystem.Embedded;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class VirtualFileProviderList : List<IFileProvider>
{
    private const string ResourceName = "Microsoft.Extensions.FileProviders.Embedded.Manifest.xml";

    /// <summary>
    /// 添加程序集中指定根目录下的所有嵌入资源。（构建类型是：EmbeddedResource）
    /// </summary>
    /// <param name="root">虚拟根目录</param>
    /// <typeparam name="TModule"></typeparam>
    public void Add<TModule>([CanBeNull] string root = null) where TModule : FakeModule
    {
        var assembly = typeof(TModule).Assembly;

        var fileProvider = CreateFileProvider(
            assembly,
            root
        );

        Add(fileProvider);
    }

    private IFileProvider CreateFileProvider(Assembly assembly, [CanBeNull] string root)
    {
        ThrowHelper.ThrowIfNull(assembly, nameof(assembly));

        // 有关给定资源如何持久化的信息
        var info = assembly.GetManifestResourceInfo(ResourceName);

        if (info != null)
            return root == null
                ? new ManifestEmbeddedFileProvider(assembly)
                : new ManifestEmbeddedFileProvider(assembly, root);

        return new FakeEmbeddedFileProvider(assembly, root);
    }
}