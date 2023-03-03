using System.Threading;
using Fake.Threading;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.Http;

public class HttpContextCancellationTokenProvider : CancellationTokenProviderBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCancellationTokenProvider(IAmbientScopeProvider<CancellationToken> ambientScopeProvider,
        IHttpContextAccessor httpContextAccessor
    ) : base(ambientScopeProvider)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override CancellationToken Token
    {
        get
        {
            if (CurrentToken == default) return CurrentToken;
            return _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
        }
    }
}