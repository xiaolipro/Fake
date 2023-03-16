using Fake;
using Fake.VirtualFileSystem;
using Fake.VirtualFileSystem.Embedded;
using JetBrains.Annotations;

namespace Microsoft.Extensions.FileProviders;

public static class FakeFileInfoExtensions
{
    [CanBeNull]
    public static string GetVirtualOrPhysicalPathOrNull([NotNull] this IFileInfo fileInfo)
    {
        ThrowHelper.ThrowIfNull(fileInfo, nameof(fileInfo));

        if (fileInfo is FakeEmbeddedFileInfo embeddedResourceFileInfo)
        {
            return embeddedResourceFileInfo.VirtualPath;
        }

        if (fileInfo is InMemoryFileInfo inMemoryFileInfo)
        {
            return inMemoryFileInfo.VirtualPath;
        }

        return fileInfo.PhysicalPath;
    }
}