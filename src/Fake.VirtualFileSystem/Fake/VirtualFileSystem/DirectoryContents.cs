using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class DirectoryContents : IDirectoryContents
{
    private readonly IEnumerable<IFileInfo> _fileEntries;

    public DirectoryContents([NotNull] IEnumerable<IFileInfo> fileEntries)
    {
        _fileEntries = fileEntries;
    }
    
    public bool Exists => true;
    
    public IEnumerator<IFileInfo> GetEnumerator()
    {
        return _fileEntries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _fileEntries.GetEnumerator();
    }
}