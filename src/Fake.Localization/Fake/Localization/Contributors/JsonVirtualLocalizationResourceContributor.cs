﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;

namespace Fake.Localization.Contributors;

public class JsonVirtualLocalizationResourceContributorBase(string virtualPath)
    : VirtualFileLocalizationResourceContributorBase(virtualPath)
{
    private static readonly JsonSerializerOptions? DeserializeOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true, // 属性名称不区分大小写
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, // 字典key使用驼峰命名
        ReadCommentHandling = JsonCommentHandling.Skip, // 跳过注释
        AllowTrailingCommas = true // 允许尾随逗号
    };

    protected override bool CanParse(IFileInfo file)
    {
        return file.Name.EndsWith(".json");
    }

    protected override ILocalizedStringContainer CreateLocalizedStringContainer(string content, string path)
    {
        JsonVirtualLocalizationResourceFile? jsonVirtualFile;
        try
        {
            jsonVirtualFile =
                JsonSerializer.Deserialize<JsonVirtualLocalizationResourceFile>(content, DeserializeOptions);
        }
        catch (JsonException ex)
        {
            throw new FakeException(path + "无法解析json字符串。" + ex.Message);
        }

        var culture = jsonVirtualFile?.Culture;
        if (culture.IsNullOrWhiteSpace())
        {
            throw new FakeException(path + "本地化json文件Culture不能空");
        }

        var dic = new Dictionary<string, LocalizedString>();
        foreach (var item in jsonVirtualFile!.Texts)
        {
            if (item.Key.IsNullOrWhiteSpace())
            {
                throw new FakeException($"{path}存在空key，value是：{item.Value}");
            }

            dic[item.Key] = new LocalizedString(item.Key, item.Value);
        }

        return new LocalizedStringContainer(path, culture!, dic);
    }
}