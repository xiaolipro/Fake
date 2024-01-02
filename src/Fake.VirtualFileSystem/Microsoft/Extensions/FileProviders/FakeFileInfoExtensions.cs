using System;
using System.IO;
using System.Text;
using Fake;
using Fake.VirtualFileSystem;
using Fake.VirtualFileSystem.Embedded;

namespace Microsoft.Extensions.FileProviders;

public static class FakeFileInfoExtensions
{
    /// <summary>
    /// 文件内容读取为字符串，使用指定编码。
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">fileInfo is null</exception>
    public static string ReadAsString(this IFileInfo fileInfo, Encoding encoding)
    {
        ThrowHelper.ThrowIfNull(fileInfo, nameof(fileInfo));

        using var stream = fileInfo.CreateReadStream();
        using var streamReader = new StreamReader(stream, encoding, true);
        return streamReader.ReadToEnd();
    }

    public static string? GetVirtualOrPhysicalPathOrNull(this IFileInfo fileInfo)
    {
        ThrowHelper.ThrowIfNull(fileInfo, nameof(fileInfo));

        if (fileInfo is EmbeddedFileInfo embeddedResourceFileInfo)
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