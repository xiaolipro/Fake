using Fake.Localization.Resources;
using Fake.Testing;
using Localization;
using Microsoft.Extensions.Localization;
using Shouldly;
using Xunit;

namespace Fake.Localization;

public sealed class FakeStringLocalizerFactoryTest: FakeIntegrationTest<FakeLocalizationTestModule>
{
    private readonly IStringLocalizerFactory _localizerFactory;
    
    public FakeStringLocalizerFactoryTest()
    {
        _localizerFactory = GetRequiredService<IStringLocalizerFactory>();
    }

    [Fact]
    public void 内部的StringLocalizer一定是InMemoryStringLocalizer()
    {
        var internalLocalizer = _localizerFactory.Create(typeof(LocalizationTestResource)).GetInternalLocalizer();
        internalLocalizer.ShouldNotBeNull();
        internalLocalizer.ShouldBeOfType<InMemoryStringLocalizer>();
    }
    
    [Fact]
    public void 可以实现本地化()
    {
        var localizer = _localizerFactory.Create(typeof(LocalizationTestResource));
        
        localizer["Hi"].Value.ShouldBe("你好");
    }
    
    [Fact]
    public void 本地化资源可以继承()
    {
        var localizer = _localizerFactory.Create(typeof(LocalizationTestResource));
        
        localizer["ThisFieldIsRequired"].Value.ShouldBe("此字段是必填字段");
        localizer["DefaultLanguage"].Value.ShouldBe("默认语言");
    }

    [Fact]
    public void 没有给定资源类型的也能支持()
    {
        var localizer = _localizerFactory.CreateByResourceName("LocalizationTestCountryNames");

        localizer.ShouldNotBeNull();
        localizer["USA"].Value.ShouldBe("美利坚");
    }
}