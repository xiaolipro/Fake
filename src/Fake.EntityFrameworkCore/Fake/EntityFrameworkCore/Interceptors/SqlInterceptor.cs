using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Fake.EntityFrameworkCore.Interceptors;

public class SqlInterceptor: DbCommandInterceptor
{
    private readonly ILogger<SqlInterceptor> _logger;

    public SqlInterceptor(ILogger<SqlInterceptor> logger)
    {
        _logger = logger;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        LogSql(command.CommandText, command.Parameters);
        return base.ReaderExecuting(command, eventData, result);
    }

    private void LogSql(string sql, DbParameterCollection parameters)
    {
        _logger.LogInformation($"SQL: {sql}");

        foreach (var parameter in parameters)
        {
            _logger.LogInformation($"Parameter: {parameter}");
        }
    }
}