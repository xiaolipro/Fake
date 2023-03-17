﻿using Fake.Testing;
using Localization;
using Microsoft.Extensions.Localization;
using Shouldly;
using Xunit;

public sealed class FakeStringLocalizerTest:FakeIntegrationTest<FakeLocalizationTestModule>
{
    private readonly IStringLocalizer<LocalizationTestResource> _localizer;
    
    public FakeStringLocalizerTest()
    {
        _localizer = GetRequiredService<IStringLocalizer<LocalizationTestResource>>();
    }

    [Theory]
    [InlineData("Hi","你好")]
    public void 可以正常本地化(string key, string value)
    {
        _localizer[key].Value.ShouldBe(value);
    }
}