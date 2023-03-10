namespace Fake.VirtualFileSystem;

public class FakeVirtualFileSystemOptions
{
    public VirtualFileSetList FileSets { get; }

    public FakeVirtualFileSystemOptions()
    {
        FileSets = new VirtualFileSetList();
    }
}