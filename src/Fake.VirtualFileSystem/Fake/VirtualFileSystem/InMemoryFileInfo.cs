using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class InMemoryFileInfo(string virtualPath, byte[] fileContent) : IFileInfo
{
    public InMemoryFileInfo(string virtualPath, string fileContent)
        : this(virtualPath, fileContent.ToBytes(Encoding.UTF8))
    {
    }

    public string VirtualPath { get; } = virtualPath;

    public string? PhysicalPath => null;

    public bool Exists => true;

    public long Length => fileContent.Length;
    public string Name { get; } = Path.GetFileName(virtualPath);

    public DateTimeOffset LastModified { get; } = DateTimeOffset.Now;

    public bool IsDirectory => false;

    public Stream CreateReadStream()
    {
        return new MemoryStream(fileContent, false);
    }
}