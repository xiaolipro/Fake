using System.Collections.Generic;
using Fake.Identity.Security.Claims;

namespace Fake.AspNetCore.Security.Claims;

public class FakeClaimsMapOptions
{
    public Dictionary<string, string> Maps { get; set; }

    public FakeClaimsMapOptions()
    {
        Maps = new Dictionary<string, string>
        {
            { "sub", FakeClaimTypes.UserId },
            { "role", FakeClaimTypes.Role },
            { "email", FakeClaimTypes.Email },
            { "name", FakeClaimTypes.UserName },
        };
    }
}