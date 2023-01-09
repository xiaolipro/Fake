using System.IO;

namespace Microsoft.Extensions.Configuration;

public static class BangConfigurationHelper
{
    public static IConfigurationRoot Build(
        BangConfigurationBuilderOptions options = null,
        Action<IConfigurationBuilder> builderAction = null)
    {
        options ??= new BangConfigurationBuilderOptions();

        if (options.BasePath.IsNullOrEmpty())
        {
            options.BasePath = Directory.GetCurrentDirectory();
        }

        // 根据当前环境添加json文件
        var builder = new ConfigurationBuilder()
            .SetBasePath(options.BasePath)
            .AddJsonFile(options.FileName + ".json", optional: options.Optional, reloadOnChange: options.ReloadOnChange);

        if (!options.EnvironmentName.IsNullOrEmpty())
        {
            builder = builder.AddJsonFile($"{options.FileName}.{options.EnvironmentName}.json", optional: options.Optional, reloadOnChange: options.ReloadOnChange);
        }

        // 非开发环境不使用用户机密
        if (options.EnvironmentName == "Development")
        {
            if (options.UserSecretsId != null)
            {
                builder.AddUserSecrets(options.UserSecretsId);
            }
        }

        // 命令行参数
        if (options.CommandLineArgs != null)
        {
            builder = builder.AddCommandLine(options.CommandLineArgs);
        }

        builderAction?.Invoke(builder);

        return builder.Build();
    }
}