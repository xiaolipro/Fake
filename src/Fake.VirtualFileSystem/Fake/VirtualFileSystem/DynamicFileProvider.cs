using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Fake.VirtualFileSystem;

/// <summary>
/// 当前实现仅支持文件监视，不支持目录或通配符监视。
/// </summary>
public class DynamicFileProvider : AbstractInMemoryFileProvider, IDynamicFileProvider
{
    private readonly ConcurrentDictionary<string, IFileInfo> _dynamicFiles = new();
    protected override IDictionary<string, IFileInfo> Files => _dynamicFiles;

    protected ConcurrentDictionary<string, ChangeTokenInfo> FilePathTokenPairs { get; } =
        new(StringComparer.OrdinalIgnoreCase); // 路径忽略大小写

    public bool AddOrUpdate(IFileInfo fileInfo)
    {
        var path = fileInfo.GetVirtualOrPhysicalPathOrNull();
        if (path == null) return false;
        _dynamicFiles.AddOrUpdate(path, fileInfo, (_, _) => fileInfo);

        return ReportChange(path);
    }

    public bool Delete(string path)
    {
        if (!_dynamicFiles.TryRemove(path, out _)) return false;

        return ReportChange(path);
    }

    public override IChangeToken Watch(string filter)
    {
        // TODO：不支持目录和通配符
        return GetOrAddChangeToken(filter);
    }

    private IChangeToken GetOrAddChangeToken(string path)
    {
        if (FilePathTokenPairs.TryGetValue(path, out var tokenInfo))
        {
            return tokenInfo.ChangeToken;
        }

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
        tokenInfo = new ChangeTokenInfo(cancellationTokenSource, cancellationChangeToken);
        tokenInfo = FilePathTokenPairs.GetOrAdd(path, tokenInfo);

        return tokenInfo.ChangeToken;
    }

    private bool ReportChange(string path)
    {
        if (!FilePathTokenPairs.TryRemove(path, out var tokenInfo))
        {
            return false;
        }

        tokenInfo.TokenSource.Cancel();
        return true;
    }


    protected readonly struct ChangeTokenInfo(
        CancellationTokenSource tokenSource,
        CancellationChangeToken changeToken)
    {
        public CancellationTokenSource TokenSource { get; } = tokenSource;

        public CancellationChangeToken ChangeToken { get; } = changeToken;
    }
}