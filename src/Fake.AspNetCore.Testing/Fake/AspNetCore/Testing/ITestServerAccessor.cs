using Microsoft.AspNetCore.TestHost;

namespace Fake.AspNetCore.Testing;

public interface ITestServerAccessor
{
    TestServer Server { get; set; }
}