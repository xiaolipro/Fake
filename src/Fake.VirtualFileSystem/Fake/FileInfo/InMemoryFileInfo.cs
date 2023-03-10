using System;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class InMemoryFileInfo: IFileInfo
{
    public string VirtualPath { get; }

    public string PhysicalPath => null;
    
    public bool Exists => true;

    public long Length => _fileContent.Length;
    public string Name { get; }
    public DateTimeOffset LastModified { get; }

    public bool IsDirectory => false;

    private readonly byte[] _fileContent;


    public InMemoryFileInfo(string virtualPath, byte[] fileContent, string name)
    {
        VirtualPath = virtualPath;
        Name = name;
        _fileContent = fileContent;
        LastModified = DateTimeOffset.Now;
    }

    public Stream CreateReadStream()
    {
        return new MemoryStream(_fileContent, false);
    }
}