using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class VirtualDirectoryInfo(
    string physicalPath,
    string name,
    DateTimeOffset lastModified)
    : IFileInfo
{
    public bool Exists => true;
    public long Length => 0;

    public string PhysicalPath { get; } = physicalPath;

    public string Name { get; } = name;

    public DateTimeOffset LastModified { get; } = lastModified;

    public bool IsDirectory => true;

    public Stream CreateReadStream()
    {
        throw new InvalidOperationException();
    }
}