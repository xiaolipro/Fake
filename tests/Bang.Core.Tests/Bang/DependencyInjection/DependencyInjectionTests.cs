using Bang.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Bang.DependencyInjection;

public class DependencyInjectionTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public DependencyInjectionTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void 默认不注册自己()
    {
        using (var application = BangApplicationFactory.Create<IndependentModule>())
        {
            application.Configure();
            var instance = application.ServiceProvider.GetService<A>();
            instance.ShouldBeNull();
            
            var instance2 = application.ServiceProvider.GetService<IA>();
            instance2.ShouldNotBeNull();
        }
    }
    
    [Fact]
    public void 强行暴露()
    {
        using (var application = BangApplicationFactory.Create<IndependentModule>())
        {
            application.Configure();
            var instance = application.ServiceProvider.GetService<MyA>();
            instance.ShouldNotBeNull();

            _testOutputHelper.WriteLine(instance.GetHashCode().ToString());
            
            var instance2 = application.ServiceProvider.GetService<IA>();
            instance2.ShouldNotBeNull();
            _testOutputHelper.WriteLine(instance2.GetHashCode().ToString());
        }
    }
    
    [Fact]
    public void 替换实现()
    {
        using (var application = BangApplicationFactory.Create<IndependentModule>())
        {
            application.Configure();
            var a = application.ServiceProvider.GetRequiredService<IA>();
            a.Increment();
            var b = application.ServiceProvider.GetRequiredService<MyB>();
            b.Increment();
            var c = application.ServiceProvider.GetRequiredService<MyA>();
            c.Increment();
            var d = application.ServiceProvider.GetRequiredService<A>();
            d.Increment();

            _testOutputHelper.WriteLine(a.GetV().ToString());
            _testOutputHelper.WriteLine(b.GetV().ToString());
            _testOutputHelper.WriteLine(c.GetV().ToString());
            _testOutputHelper.WriteLine(d.GetV().ToString());
        }
    }
}

public class MyB : IA, ISingleton
{
    private int x;
    
    public void Increment()
    {
        x++;
    }

    public int GetV()
    {
        return x;
    }
}

public class MyA : IA, IScoped
{
    private int x;
    
    public void Increment()
    {
        x++;
    }

    public int GetV()
    {
        return x;
    }
}

public interface IA
{
    void Increment();

    int GetV();
}

public class A : IA, ISingleton
{
    private int _num;
    
    public void Increment()
    {
        _num++;
    }

    public int GetV()
    {
        return _num;
    }
}