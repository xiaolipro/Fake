using System.IO;
using System.Text;

namespace System;

public static class FakeStreamExtensions
{
    /// <summary>
    /// 读取流，返回字节数组
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="bufferSize">缓冲区字节大小</param>
    /// <returns></returns>
    public static byte[] ReadAsBytes(this Stream stream, int bufferSize = 81920)
    {
        using (var memoryStream = new MemoryStream())
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            stream.CopyTo(memoryStream, bufferSize);
            return memoryStream.ToArray();
        }
    }

    /// <summary>
    /// 异步读取流，返回字节数组
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="bufferSize">缓冲区字节大小</param>
    /// <returns></returns>
    public static async Task<byte[]> ReadAsBytesAsync(this Stream stream, CancellationToken cancellationToken = default,
        int bufferSize = 81920)
    {
        using (var memoryStream = new MemoryStream())
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            await stream.CopyToAsync(memoryStream, cancellationToken, bufferSize);
            return memoryStream.ToArray();
        }
    }

    /// <summary>
    /// 将一个流复制到另一个流
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="destination"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static Task CopyToAsync(this Stream stream, Stream destination, CancellationToken cancellationToken,
        int bufferSize = 81920)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        return stream.CopyToAsync(
            destination,
            bufferSize,
            cancellationToken
        );
    }

    public static string ReadAsUTF8String(this Stream stream)
    {
        var bytes = stream.ReadAsBytes();
        
        // 跳过bom，防止乱码
        var skipCount = HasBom(bytes) ? 3 : 0;

        return Encoding.UTF8.GetString(bytes, skipCount, bytes.Length - skipCount);
    }

    /*
     * BOM(Byte Order Mark)，是UTF编码方案里用于标识编码的标准标记，在UTF-16里本来是FF FE，变成UTF-8就成了EF BB BF。
     * 这个标记是可选的，因为UTF8字节没有顺序，所以它可以被用来检测一个字节流是否是UTF-8编码的。
     * 微软在自己的UTF-8格式的文本文件之前加上了EF BB BF三个字节, windows上面的notepad等程序就是根据这三个字节来确定一个文本文件
     * 是ASCII的还是UTF-8的, 然而这个只是微软暗自作的标记, 其它平台上并没有对UTF-8文本文件做个这样的标记。
     */
    private static bool HasBom(byte[] bytes)
    {
        if (bytes.Length < 3) return false;

        return bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;
    }
}