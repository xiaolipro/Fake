using System.Threading;
using Fake.Threading;
using Microsoft.AspNetCore.Http;

namespace Fake.AspNetCore.Http;

public class HttpContextCancellationTokenProvider(
    IAmbientScopeProvider<CancellationToken> ambientScopeProvider,
    IHttpContextAccessor httpContextAccessor)
    : CancellationTokenProviderBase(ambientScopeProvider)
{
    public override CancellationToken Token
    {
        get
        {
            if (CurrentToken == default) return CurrentToken;
            return httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
        }
    }
}