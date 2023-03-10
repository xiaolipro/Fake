using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Fake.Embedded;
using Fake.FileInfo;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;

namespace Fake.VirtualFileSystem;

public class FakeEmbeddedFileProvider : DictionaryFileProvider
{
    [NotNull]
    public Assembly Assembly { get; }

    [CanBeNull]
    public string BaseNamespace { get; }
    
    protected override IDictionary<string, IFileInfo> FileDictionary => _fileDic.Value;
    private readonly Lazy<Dictionary<string, IFileInfo>> _fileDic;
    
    public FakeEmbeddedFileProvider(
        [NotNull] Assembly assembly,
        [CanBeNull] string baseNamespace = null)
    {
        ThrowHelper.ThrowIfNull(assembly, nameof(assembly));

        Assembly = assembly;
        BaseNamespace = baseNamespace;

        _fileDic = new Lazy<Dictionary<string, IFileInfo>>(
            () =>
            {
                var fileDic = new Dictionary<string, IFileInfo>(StringComparer.OrdinalIgnoreCase);
                AddFiles(fileDic);
                return fileDic;
            },
            true
        );
    }

    
    public void AddFiles(Dictionary<string, IFileInfo> fileDic)
    {
        var lastModificationTime = GetLastModificationTime();

        foreach (var resourcePath in Assembly.GetManifestResourceNames())
        {
            if (!BaseNamespace.IsNullOrEmpty() && !resourcePath.StartsWith(BaseNamespace))
            {
                continue;
            }

            var fullPath = ConvertToRelativePath(resourcePath).StartsWithOrAppend("/");

            AddDirectoriesRecursively(fileDic, fullPath.Substring(0, fullPath.LastIndexOf('/')), lastModificationTime);

            fileDic[fullPath] = new EmbeddedResourceFileInfo(
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
    
    private string ConvertToRelativePath(string resourceName)
    {
        if (!BaseNamespace.IsNullOrEmpty())
        {
            resourceName = resourceName.Substring(BaseNamespace.Length + 1);
        }

        var pathParts = resourceName.Split('.');
        if (pathParts.Length <= 2)
        {
            return resourceName;
        }

        // a.b.c.txt => a/b/c.txt
        var folder = pathParts.Take(pathParts.Length - 2).JoinAsString("/");
        var fileName = pathParts.Skip(pathParts.Length - 2).JoinAsString(".");
        return folder + "/" + fileName;
    }
    
    private static void AddDirectoriesRecursively(Dictionary<string, IFileInfo> fileDic, string directoryPath, DateTimeOffset lastModificationTime)
    {
        if (fileDic.ContainsKey(directoryPath))
        {
            return;
        }

        fileDic[directoryPath] = new VirtualDirectoryFileInfo(
            directoryPath,
            CalculateFileName(directoryPath),
            lastModificationTime
        );

        if (directoryPath.Contains("/"))
        {
            AddDirectoriesRecursively(fileDic, directoryPath.Substring(0, directoryPath.LastIndexOf('/')), lastModificationTime);
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