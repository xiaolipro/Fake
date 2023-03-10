using System.Collections.Generic;
using System.Reflection;
using Fake.VirtualFileSystem.Embedded;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Fake.VirtualFileSystem;

public class VirtualFileSetList : List<VirtualFileSet>
{
    public ILogger<VirtualFileSetList> Logger { get; set; }
    private const string ResourceName = "Microsoft.Extensions.FileProviders.Embedded.Manifest.xml";
    public void AddEmbedded<TModule>([CanBeNull] string baseNamespace = null,
        [CanBeNull] string baseFolder = null)
    {
        var assembly = typeof(TModule).Assembly;

        var fileProvider = CreateFileProvider(
            assembly,
            baseNamespace,
            baseFolder
        );
    }

    private IFileProvider CreateFileProvider([NotNull] Assembly assembly, [CanBeNull] string baseNamespace,
        [CanBeNull] string baseFolder)
    {
        ThrowHelper.ThrowIfNull(assembly, nameof(assembly));
        
        // 有关给定资源如何持久化的信息
        var info = assembly.GetManifestResourceInfo(ResourceName);

        if (info != null)
            return baseFolder == null
                ? new ManifestEmbeddedFileProvider(assembly)
                : new ManifestEmbeddedFileProvider(assembly, baseFolder);
        
        Logger.LogWarning($"找不到{ResourceName}，正在使用{nameof(FakeEmbeddedFileProvider)}");
        return new FakeEmbeddedFileProvider(assembly, baseNamespace);
    }
}