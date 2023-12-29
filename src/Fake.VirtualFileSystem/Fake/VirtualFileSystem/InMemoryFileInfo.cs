using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class InMemoryFileInfo(string virtualPath, byte[] fileContent, string name) : IFileInfo
{
    public string VirtualPath { get; } = virtualPath;

    public string? PhysicalPath => null;

    public bool Exists => true;

    public long Length => fileContent.Length;
    public string Name { get; } = name;
    public DateTimeOffset LastModified { get; } = DateTimeOffset.Now;

    public bool IsDirectory => false;

    public Stream CreateReadStream()
    {
        return new MemoryStream(fileContent, false);
    }
}