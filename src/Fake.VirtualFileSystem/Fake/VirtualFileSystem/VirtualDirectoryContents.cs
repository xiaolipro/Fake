using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class VirtualDirectoryContents(IEnumerable<IFileInfo> fileEntries) : IDirectoryContents
{
    public bool Exists => true;

    public IEnumerator<IFileInfo> GetEnumerator()
    {
        return fileEntries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return fileEntries.GetEnumerator();
    }
}