using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem.Embedded;

public class FakeEmbeddedDirectoryFileInfo: IFileInfo
{
    public bool Exists => true;

    public long Length => 0;

    public string PhysicalPath { get; }

    public string Name { get; }

    public DateTimeOffset LastModified { get; }

    public bool IsDirectory => true;

    public FakeEmbeddedDirectoryFileInfo(string physicalPath, string name, DateTimeOffset lastModified)
    {
        PhysicalPath = physicalPath;
        Name = name;
        LastModified = lastModified;
    }

    public Stream CreateReadStream()
    {
        throw new InvalidOperationException();
    }
}