using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem.Embedded;

public class FakeEmbeddedFileProvider : AbstractInMemoryFileProvider
{
    protected virtual Assembly Assembly { get; }

    protected virtual string Root { get; }

    protected override IDictionary<string, IFileInfo?> Files => _fileDic.Value;
    private readonly Lazy<Dictionary<string, IFileInfo?>> _fileDic;

    public FakeEmbeddedFileProvider(Assembly assembly, string root = null)
    {
        ThrowHelper.ThrowIfNull(assembly, nameof(assembly));

        Assembly = assembly;
        Root = root?.Trim('/').Replace("/", ".");

        _fileDic = new Lazy<Dictionary<string, IFileInfo?>>(
            () =>
            {
                var fileDic = new Dictionary<string, IFileInfo>(StringComparer.OrdinalIgnoreCase);
                AddFiles(fileDic);
                return fileDic;
            },
            true
        );
    }


    public virtual void AddFiles(Dictionary<string, IFileInfo> fileDic)
    {
        var lastModificationTime = GetLastModificationTime();

        foreach (var resourcePath in Assembly.GetManifestResourceNames())
        {
            if (Root.IsNotNullOrWhiteSpace() && !resourcePath.StartsWith(Root))
            {
                continue;
            }

            // 资产名称a.b.c->目录层级/a/b/c
            var fullPath = ConvertToRelativePath(resourcePath);

            // 添加虚拟目录
            AddVirtualDirectoriesRecursively(fileDic, fullPath.Substring(0, fullPath.LastIndexOf('/')),
                lastModificationTime);

            // 添加虚拟文件
            fileDic[fullPath] = new FakeEmbeddedFileInfo(
                Assembly,
                resourcePath,
                fullPath,
                CalculateFileName(fullPath),
                lastModificationTime
            );
        }
    }


    private DateTimeOffset GetLastModificationTime()
    {
        var lastModified = DateTimeOffset.UtcNow;

        if (!string.IsNullOrEmpty(Assembly.Location))
        {
            try
            {
                lastModified = File.GetLastWriteTimeUtc(Assembly.Location);
            }
            catch (PathTooLongException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        return lastModified;
    }

    private string ConvertToRelativePath(string resourcePath)
    {
        if (Root.IsNotNullOrWhiteSpace())
        {
            resourcePath = resourcePath.Substring(Root.Length + 1);
        }

        var pathParts = resourcePath.Split('.');
        if (pathParts.Length <= 2)
        {
            return resourcePath;
        }

        // a.b.c.txt => a/b/c.txt
        var folder = pathParts.Take(pathParts.Length - 2).JoinAsString("/");
        var fileName = pathParts.Skip(pathParts.Length - 2).JoinAsString(".");
        return folder + "/" + fileName;
    }

    private static void AddVirtualDirectoriesRecursively(Dictionary<string, IFileInfo> fileDic, string directoryPath,
        DateTimeOffset lastModificationTime)
    {
        if (fileDic.ContainsKey(directoryPath))
        {
            return;
        }

        fileDic[directoryPath] = new FakeEmbeddedDirectoryFileInfo(
            directoryPath,
            CalculateFileName(directoryPath),
            lastModificationTime
        );

        if (directoryPath.Contains("/"))
        {
            AddVirtualDirectoriesRecursively(fileDic, directoryPath.Substring(0, directoryPath.LastIndexOf('/')),
                lastModificationTime);
        }
    }


    private static string CalculateFileName(string filePath)
    {
        if (!filePath.Contains("/"))
        {
            return filePath;
        }

        return filePath.Substring(filePath.LastIndexOf("/", StringComparison.Ordinal) + 1);
    }
}