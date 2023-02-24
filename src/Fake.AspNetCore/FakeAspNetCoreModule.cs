using Fake.Identity;
using Fake.Modularity;

// ReSharper disable once CheckNamespace
namespace Fake.AspNetCore;


[DependsOn(typeof(FakeIdentityModule))]
public class FakeAspNetCoreModule:FakeModule
{
    
}