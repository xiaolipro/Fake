using Fake.Authorization.Services;

namespace Fake.Authorization;

public class AuthorizationTests : AuthorizationTestBase
{
    private readonly SimpleService _simpleService;

    public AuthorizationTests()
    {
        _simpleService = GetRequiredService<SimpleService>();
    }
}