using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

/// <summary>
/// 动态文件供应商，用于在运行时添加修改或删除文件（对于Fake文件系统而言）。
/// </summary>
public interface IDynamicFileProvider : IFileProvider
{
    /// <summary>
    /// 添加或更新文件
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    bool AddOrUpdate(IFileInfo fileInfo);

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    bool Delete(string filePath);
}