using Fake.Testing;
using Localization;
using Microsoft.Extensions.Localization;
using Shouldly;
using Xunit;

public class FakeLocalization_Tests: FakeIntegrationTest<FakeLocalizationTestModule>
{
    private readonly IStringLocalizer<LocalizationTestResource> _localizer;
    
    public FakeLocalization_Tests()
    {
        _localizer = GetRequiredService<IStringLocalizer<LocalizationTestResource>>();
    }

    [Fact]
    public void 基础用法()
    {
        _localizer["Hi"].Value.ShouldBe("");
    }
}