using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class VirtualFileSet
{
    public IFileProvider FileProvider { get; }

    public VirtualFileSet([NotNull] IFileProvider fileProvider)
    {
        ThrowHelper.ThrowIfNull(fileProvider, nameof(fileProvider));
        FileProvider = fileProvider;
    }
}