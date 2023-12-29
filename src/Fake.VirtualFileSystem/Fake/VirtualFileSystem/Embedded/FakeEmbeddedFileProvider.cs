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

    protected virtual string? Root { get; }

    protected override IDictionary<string, IFileInfo> Files => _files.Value;
    private readonly Lazy<Dictionary<string, IFileInfo>> _files;

    public FakeEmbeddedFileProvider(Assembly assembly, string? root = null)
    {
        ThrowHelper.ThrowIfNull(assembly, nameof(assembly));

        Assembly = assembly;
        Root = root?.Trim('/').Replace("/", ".");

        _files = new Lazy<Dictionary<string, IFileInfo>>(
            () =>
            {
                var fileDic = new Dictionary<string, IFileInfo>(StringComparer.OrdinalIgnoreCase);
                AddFiles(fileDic);
                return fileDic;
            },
            true
        );
    }


    public virtual void AddFiles(Dictionary<string, IFileInfo> files)
    {
        var lastModificationTime = GetLastModificationTime();

        foreach (var resourcePath in Assembly.GetManifestResourceNames())
        {
            if (!Root.IsNullOrEmpty() && !resourcePath.StartsWith(Root!))
            {
                continue;
            }

            // 资产名称a.b.c->目录层级/a/b/c
            var fullPath = ConvertToRelativePath(resourcePath).StartsWithAppend("/");

            // 添加虚拟目录
            AddVirtualDirectoriesRecursively(files, fullPath.Substring(0, fullPath.LastIndexOf('/')),
                lastModificationTime);

            // 添加虚拟文件
            files[fullPath] = new EmbeddedFileInfo(
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
        if (!Root.IsNullOrEmpty())
        {
            resourcePath = resourcePath.Substring(Root!.Length + 1);
        }

        var paths = resourcePath.Split('.');
        if (paths.Length <= 2)
        {
            return resourcePath;
        }

        // a.b.c.txt => a/b/c.txt
        var folder = paths.Take(paths.Length - 2).JoinAsString("/");
        var fileName = paths.Skip(paths.Length - 2).JoinAsString(".");
        return folder + "/" + fileName;
    }

    private static void AddVirtualDirectoriesRecursively(Dictionary<string, IFileInfo> files, string directoryPath,
        DateTimeOffset lastModificationTime)
    {
        if (files.ContainsKey(directoryPath))
        {
            return;
        }

        files[directoryPath] = new VirtualDirectoryFileInfo(
            directoryPath,
            CalculateFileName(directoryPath),
            lastModificationTime
        );

        if (directoryPath.Contains("/"))
        {
            AddVirtualDirectoriesRecursively(files, directoryPath.Substring(0, directoryPath.LastIndexOf('/')),
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