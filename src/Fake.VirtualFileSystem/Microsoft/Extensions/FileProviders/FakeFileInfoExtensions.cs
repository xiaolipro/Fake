using Fake;
using Fake.Embedded;
using Fake.VirtualFileSystem;
using JetBrains.Annotations;

namespace Microsoft.Extensions.FileProviders;

public static class FakeFileInfoExtensions
{
    [CanBeNull]
    public static string GetVirtualOrPhysicalPathOrNull([NotNull] this IFileInfo fileInfo)
    {
        ThrowHelper.ThrowIfNull(fileInfo, nameof(fileInfo));

        if (fileInfo is EmbeddedResourceFileInfo embeddedResourceFileInfo)
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