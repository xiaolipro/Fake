using System;
using System.Text;
using Fake.Localization;
using Fake.Testing;
using Fake.VirtualFileSystem;
using Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shouldly;
using Xunit;

public sealed class FakeStringLocalizerFactoryTests: FakeIntegrationTest<FakeLocalizationTestModule>
{
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly VirtualFileProvider _virtualFileProvider;
    
    public FakeStringLocalizerFactoryTests()
    {
        _localizerFactory = GetRequiredService<IStringLocalizerFactory>();
        _virtualFileProvider = ServiceProvider.GetRequiredService<VirtualFileProvider>();
    }

    [Fact]
    public void 内部的StringLocalizer一定是InMemoryStringLocalizer()
    {
        var internalLocalizer = _localizerFactory.Create(typeof(LocalizationTestResource)).GetInternalLocalizer();
        internalLocalizer.ShouldNotBeNull();
        internalLocalizer.ShouldBeOfType<InMemoryStringLocalizer>();
    }
    
    [Fact]
    void 嵌入文件()
    {
        //action
        var resource = _virtualFileProvider.GetFileInfo("Localization/Resources/zh.json");
        
        //assert
        resource.ShouldNotBeNull();
        resource.Exists.ShouldBeTrue();
        
        using (var stream = resource.CreateReadStream())
        {
            Encoding.UTF8.GetString(stream.ReadAsBytes()).ShouldBe("123");
        }
    }
    
    
    [Fact]
    public void 可以实现本地化()
    {
        var internalLocalizer = _localizerFactory.Create(typeof(LocalizationTestResource));
        
        internalLocalizer["Hi"].Value.ShouldBe("你好");
    }
}