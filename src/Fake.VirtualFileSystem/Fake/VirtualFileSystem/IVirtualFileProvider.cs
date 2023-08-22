using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

/// <summary>
/// 虚拟文件供应商，继承自<see cref="IFileProvider"/>，用于提供虚拟文件的访问
/// </summary>
public interface IVirtualFileProvider : IFileProvider
{
}