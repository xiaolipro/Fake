namespace Fake.VirtualFileSystem;

public class FakeVirtualFileSystemOptions
{
    public VirtualFileProviderSet FileSets { get; }

    public FakeVirtualFileSystemOptions()
    {
        FileSets = new VirtualFileProviderSet();
    }
}