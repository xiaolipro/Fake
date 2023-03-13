using System;
using System.Collections.Generic;
using System.Reflection;
using Fake.VirtualFileSystem.Embedded;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class VirtualFileProviderSet
{
    private const string ResourceName = "Microsoft.Extensions.FileProviders.Embedded.Manifest.xml";
    public List<IFileProvider> FileProviders { get; }

    public VirtualFileProviderSet()
    {
        FileProviders = new List<IFileProvider>();
    }
    
    public void AddEmbedded<TModule>([CanBeNull] string baseNamespace = null,
        [CanBeNull] string root = null)
    {
        var assembly = typeof(TModule).Assembly;
        
        var fileProvider = CreateFileProvider(
            assembly,
            baseNamespace,
            root
        );
        
        FileProviders.Add(fileProvider);
    }

    private IFileProvider CreateFileProvider([NotNull] Assembly assembly, [CanBeNull] string baseNamespace,
        [CanBeNull] string root)
    {
        ThrowHelper.ThrowIfNull(assembly, nameof(assembly));
        
        // 有关给定资源如何持久化的信息
        var info = assembly.GetManifestResourceInfo(ResourceName);

        if (info != null)
            return root == null
                ? new ManifestEmbeddedFileProvider(assembly)
                : new ManifestEmbeddedFileProvider(assembly, root);
        
        return new FakeEmbeddedFileProvider(assembly, baseNamespace);
    }
}