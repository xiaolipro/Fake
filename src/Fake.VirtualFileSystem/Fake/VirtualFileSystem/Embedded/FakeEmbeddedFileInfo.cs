using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem.Embedded;

public class FakeEmbeddedFileInfo: IFileInfo
{
    /// <summary>
    /// 虚拟路径
    /// </summary>
    public string VirtualPath { get; }
    public string PhysicalPath => null;
    
    public string Name { get; }
    public DateTimeOffset LastModified { get; }
    
    public bool Exists => true;

    private long? _length;
    public long Length
    {
        get
        {
            if (_length.HasValue) return _length.Value;

            using var stream = _assembly.GetManifestResourceStream(_resourcePath);
            return stream.Length;
        }
    }

    public bool IsDirectory => false;
    
    private readonly Assembly _assembly;
    private readonly string _resourcePath;
    
    public FakeEmbeddedFileInfo(
        Assembly assembly,
        string resourcePath,
        string virtualPath,
        string name,
        DateTimeOffset lastModified)
    {
        _assembly = assembly;
        _resourcePath = resourcePath;

        VirtualPath = virtualPath;
        Name = name;
        LastModified = lastModified;
    }
    
    public Stream CreateReadStream()
    {
        var stream = _assembly.GetManifestResourceStream(_resourcePath);

        if (!_length.HasValue && stream != null)
        {
            _length = stream.Length;
        }

        return stream;
    }


    public override string ToString()
    {
        return $"[{nameof(FakeEmbeddedFileInfo)}] {Name} ({this.VirtualPath})";
    }
}