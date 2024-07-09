using Fake.Localization.Tests.Localization;
using Fake.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shouldly;
using Xunit;

namespace Fake.Localization.Tests;

public sealed class StringLocalizerTest : ApplicationTest<FakeLocalizationTestModule>
{
    private readonly IStringLocalizer<LocalizationTestResource> _localizer;

    public StringLocalizerTest()
    {
        _localizer = ServiceProvider.GetRequiredService<IStringLocalizer<LocalizationTestResource>>();
    }

    [Theory]
    [InlineData("Hi", "你好")]
    public void 可以正常本地化(string key, string value)
    {
        _localizer[key].Value.ShouldBe(value);
    }

    [Fact]
    public void 可以切换文化()
    {
        using (CultureHelper.UseCulture("en"))
        {
            _localizer["Hi", "xx"].Value.ShouldBe("Hello xx");
        }

        _localizer["Hi", "xx"].Value.ShouldBe("你好");
    }

    [Fact]
    public void 本地化资源可以继承()
    {
        _localizer[LocalizationTestValidationResource.ThisFieldIsRequired].Value.ShouldBe("此字段是必填字段");
        _localizer[FakeLocalizationResource.DefaultLanguage].Value.ShouldBe("默认语言");
    }
}