using Fake.AspNetCore;

namespace SimpleWebDemo;

public class TestApplicationService : ApplicationService
{
    public string Hello(string name)
    {
        return $"hello {name}";
    }
}