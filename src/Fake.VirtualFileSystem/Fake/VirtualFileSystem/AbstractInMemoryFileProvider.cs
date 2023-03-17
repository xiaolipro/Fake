using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Fake.VirtualFileSystem;

public abstract class AbstractInMemoryFileProvider: IFileProvider
{
    protected abstract IDictionary<string, IFileInfo> Files { get; }
    
    public virtual IFileInfo GetFileInfo(string subpath)
    {
        if (subpath == null) return new NotFoundFileInfo("");

        var file = Files.GetOrDefault(subpath.Trim('/'));

        if (file == null) return new NotFoundFileInfo(subpath);

        return file;
    }

    public virtual IDirectoryContents GetDirectoryContents(string subpath)
    {
        var directory = GetFileInfo(subpath);
        if (!directory.IsDirectory)
        {
            return NotFoundDirectoryContents.Singleton;
        }

        var files = new List<IFileInfo>();

        var directoryPath = subpath.Trim('/');

        foreach (var fileInfo in Files.Values)
        {
            var fullPath = fileInfo.GetVirtualOrPhysicalPathOrNull();
            
            if (fullPath == null || !fullPath.StartsWith(directoryPath) || fullPath == directoryPath)
            {
                continue;
            }

            var relativePath = fullPath.Substring(directoryPath.Length + 1);
            if (relativePath.Contains("/"))
            {
                continue;
            }

            files.Add(fileInfo);
        }

        return new DirectoryContents(files);
    }

    public virtual IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}