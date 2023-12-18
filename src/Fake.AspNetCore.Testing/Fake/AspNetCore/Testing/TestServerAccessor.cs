using Microsoft.AspNetCore.TestHost;

namespace Fake.AspNetCore.Testing;

public class TestServerAccessor : ITestServerAccessor
{
    public TestServer Server { get; set; } = null!;
}