using Fake.AspNetCore;
using Fake.DependencyInjection;

namespace SimpleWebDemo;

public class TestApplicationService : ApplicationService, ITransientDependency
{
    public string Hello(string name)
    {
        return $"hello {name}";
    }
}