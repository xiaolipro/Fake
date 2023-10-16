using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fake.Collections;
using Fake.Modularity;

namespace Fake.ObjectMapping.AutoMapper;

public class FakeAutoMapperOptions
{
    /// <summary>
    /// 配置映射器配置表达式
    /// </summary>
    public List<Action<IMapperConfigurationExpression, IServiceProvider>> Configurators { get; }

    /// <summary>
    /// 需要验证的Profile
    /// </summary>
    public ITypeList<Profile> ValidatingProfile { get; set; }
    
    public FakeAutoMapperOptions()
    {
        Configurators = new List<Action<IMapperConfigurationExpression, IServiceProvider>>();
        ValidatingProfile = new TypeList<Profile>();
    }

    /// <summary>
    /// 添加模块中的Profile和用AutoMapAttribute标记的类
    /// </summary>
    /// <param name="validate">如果validate为true，则会自动添加到验证列表中</param>
    /// <typeparam name="TModule"></typeparam>
    public void AddMaps<TModule>(bool validate = false) where TModule : IFakeModule
    {
        var assembly = typeof(TModule).Assembly;

        Configurators.Add((expression, provider) => { expression.AddMaps(assembly); });

        if (!validate) return;

        var profileTypes = assembly
            .DefinedTypes
            .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && !type.IsGenericType);

        ValidatingProfile.AddRange(profileTypes);
    }
}