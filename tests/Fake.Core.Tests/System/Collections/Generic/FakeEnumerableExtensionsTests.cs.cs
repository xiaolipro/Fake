﻿using System.IO.Compression;
using System.Text;
using Fake.Helpers;

namespace Fake.Core.Tests.System.Collections.Generic;

public class FakeEnumerableExtensionsTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public FakeEnumerableExtensionsTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void t()
    {
        string zipPath =
            @"C:\Users\23577\AppData\Local\Temp\50448458-9e24-4b2f-8ee5-e7c132717377\发票批量下载_20240611182230(1).zip";
        string extractPath = "C:\\Users\\23577\\AppData\\Local\\Temp\\50448458-9e24-4b2f-8ee5-e7c132717377\\";

        // Ensure the extraction directory exists
        Directory.CreateDirectory(extractPath);

        string encoding = "UTF-8"; // Change to the correct encoding if needed

        ExtractZipFile(zipPath, extractPath, encoding);
    }

    static void ExtractZipFile(string zipPath, string extractPath, string encoding)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding gbk = Encoding.GetEncoding(encoding);

        using (ZipArchive archive = ZipFile.OpenRead(zipPath))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string decodedFileName = gbk.GetString(Encoding.UTF8.GetBytes(entry.FullName));
                string destinationPath = Path.GetFullPath(Path.Combine(extractPath, decodedFileName));

                if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                {
                    if (entry.Name == "")
                    {
                        Directory.CreateDirectory(destinationPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                        entry.ExtractToFile(destinationPath, true);
                    }
                }
            }
        }
    }

    [Fact]
    void WithIndex()
    {
        int cnt = 0;
        foreach (var (item, index) in Enumerable.Range(1, 10).WithIndex())
        {
            index.ShouldBe(cnt);
            cnt++;
            _testOutputHelper.WriteLine(index + "：" + item);
        }
    }

    [Fact]
    void JoinAsString()
    {
        Enumerable.Range(1, 3).JoinAsString(",").ShouldBe("1,2,3");
    }

    [Fact]
    public void SortByDependencies()
    {
        var dependencies = new Dictionary<char, char[]>
        {
            { 'A', new[] { 'B', 'G' } },
            { 'B', new[] { 'C', 'E' } },
            { 'C', new[] { 'D' } },
            { 'D', Array.Empty<char>() },
            { 'E', new[] { 'C', 'F' } },
            { 'F', new[] { 'C' } },
            { 'G', new[] { 'F' } }
        };

        for (int i = 0; i < 3; i++)
        {
            var list = RandomHelper.Shuffle(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' });

            list = list.SortByDependencies(c => dependencies[c]);

            foreach (var dependency in dependencies)
            {
                foreach (var dependedValue in dependency.Value)
                {
                    // 先构建依赖项
                    list.IndexOf(dependency.Key).ShouldBeGreaterThan(list.IndexOf(dependedValue));
                }
            }
        }
    }

    [Fact]
    public void SortByDependencies有环会抛出异常()
    {
        var dependencies = new Dictionary<char, char[]>
        {
            { 'A', new[] { 'B', 'G' } },
            { 'B', new[] { 'C', 'A' } },
            { 'C', new[] { 'D' } },
            { 'D', Array.Empty<char>() },
            { 'E', new[] { 'C', 'F' } },
            { 'F', new[] { 'C' } },
            { 'G', new[] { 'F' } }
        };
        var list = RandomHelper.Shuffle(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' });

        var res = Should.Throw<ArgumentException>(() => { list = list.SortByDependencies(c => dependencies[c]); });

        _testOutputHelper.WriteLine(res.Message);
    }
}