using Fake;
using Fake.VirtualFileSystem;
using Fake.VirtualFileSystem.Embedded;

namespace Microsoft.Extensions.FileProviders;

public static class FakeFileInfoExtensions
{
    public static string GetVirtualOrPhysicalPathOrNull(this IFileInfo fileInfo)
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