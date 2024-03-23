using System.Collections.Generic;
using SqlSugar;

namespace Fake.SqlSugarCore.Abstractions;

public class SugarDbConnOptions
{
    /// <summary>
    /// 连接字符串(如果开启多租户，也就是默认库了)，必填
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// 读写分离
    /// </summary>
    public List<string> ReadConnectionStrings { get; set; } = [];

    /// <summary>
    /// 数据库类型
    /// </summary>
    public DbType DbType { get; set; }

    /// <summary>
    /// 开启种子数据
    /// </summary>
    public bool EnabledDataSeeder { get; set; } = false;

    /// <summary>
    /// 开启代码先行
    /// </summary>
    public bool EnabledCodeFirst { get; set; } = false;

    /// <summary>
    /// 开启sql日志
    /// </summary>
    public bool EnabledSqlLog { get; set; } = true;
}