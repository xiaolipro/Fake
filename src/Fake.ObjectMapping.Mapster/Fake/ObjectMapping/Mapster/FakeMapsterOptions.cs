using System.Collections.Generic;
using Fake.Modularity;
using Mapster;

namespace Fake.ObjectMapping.Mapster;

public class FakeMapsterOptions
{
    public TypeAdapterConfig TypeAdapterConfig { get; set; } = new();

    public List<IRegister> Registers { get; set; } = new();

    /// <summary>
    /// 注册模块中的IRegister
    /// </summary>
    /// <typeparam name="TModule">要注册IRegister的模块</typeparam>
    public void Scan<TModule>(bool compile = false) where TModule : IFakeModule
    {
        TypeAdapterConfig.Scan(typeof(TModule).Assembly);
        if (compile)
        {
            // 推荐在程序添加映射配置完成后调用一次 Compile 方法，可以快速验证 映射配置中是否存在错误，
            // 而不是在运行到某一行业务代码时触发错误降低效率。
            TypeAdapterConfig.Compile();
        }
    }
}