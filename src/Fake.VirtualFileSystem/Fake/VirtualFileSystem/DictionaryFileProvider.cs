using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Fake.VirtualFileSystem;

public abstract class DictionaryFileProvider: IFileProvider
{
    protected abstract IDictionary<string, IFileInfo> FileDictionary { get; }
    
    public IFileInfo GetFileInfo(string subpath)
    {
        if (subpath == null) return new NotFoundFileInfo("");

        var file = FileDictionary.GetOrDefault(subpath);

        if (file == null) return new NotFoundFileInfo(subpath);

        return file;
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var directory = GetFileInfo(subpath);
        if (!directory.IsDirectory)
        {
            return NotFoundDirectoryContents.Singleton;
        }

        var files = new List<IFileInfo>();

        var directoryPath = subpath.EndsWithOrAppend("/");

        foreach (var fileInfo in FileDictionary.Values)
        {
            var fullPath = fileInfo.GetVirtualOrPhysicalPathOrNull();
            
            if (fullPath == null || !fullPath.StartsWith(directoryPath))
            {
                continue;
            }
            
            var relativePath = fullPath.Substring(directoryPath.Length);
            
            // 跳过目录
            if (relativePath.Contains("/"))
            {
                continue;
            }

            files.Add(fileInfo);
        }

        return new DirectoryContents(files);
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}