using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem.Embedded;

public class EmbeddedFileInfo(
    Assembly assembly,
    string resourcePath,
    string virtualPath,
    string name,
    DateTimeOffset lastModified)
    : IFileInfo
{
    public bool Exists => true;

    /// <summary>
    /// 虚拟路径
    /// </summary>
    public string VirtualPath { get; } = virtualPath;

    public string? PhysicalPath => null;

    public string Name { get; } = name;
    public DateTimeOffset LastModified { get; } = lastModified;


    private long? _length;

    public long Length
    {
        get
        {
            if (_length.HasValue) return _length.Value;

            using var stream = assembly.GetManifestResourceStream(resourcePath);
            return stream!.Length;
        }
    }

    public bool IsDirectory => false;

    public Stream CreateReadStream()
    {
        var stream = assembly.GetManifestResourceStream(resourcePath);

        if (!_length.HasValue && stream != null)
        {
            _length = stream.Length;
        }

        return stream!;
    }


    public override string ToString()
    {
        return $"[{nameof(EmbeddedFileInfo)}] {Name} ({this.VirtualPath})";
    }
}