namespace Fake.AspNetCore.Http;

public interface IHttpClientInfoProvider
{
    public string? UserAgent { get; }

    public string? ClientIpAddress { get; }
}