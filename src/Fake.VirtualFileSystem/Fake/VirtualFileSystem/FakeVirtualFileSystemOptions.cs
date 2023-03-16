namespace Fake.VirtualFileSystem;

public class FakeVirtualFileSystemOptions
{
    public VirtualFileProviderList FileProviders { get; }

    public FakeVirtualFileSystemOptions()
    {
        FileProviders = new VirtualFileProviderList();
    }
}