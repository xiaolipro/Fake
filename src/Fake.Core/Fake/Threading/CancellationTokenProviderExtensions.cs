namespace Fake.Threading;

public static class CancellationTokenProviderExtensions
{
    /// <summary>
    /// 如果没有提供的值，则回退到CancellationTokenProvider来提供
    /// </summary>
    /// <param name="provider"><see cref="ICancellationTokenProvider"/></param>
    /// <param name="profferedValue">提供的值</param>
    /// <returns></returns>
    public static CancellationToken FallbackToProvider(this ICancellationTokenProvider provider, CancellationToken profferedValue = default)
    {
        return profferedValue == default || profferedValue == CancellationToken.None
            ? provider.Token
            : profferedValue;
    }
}