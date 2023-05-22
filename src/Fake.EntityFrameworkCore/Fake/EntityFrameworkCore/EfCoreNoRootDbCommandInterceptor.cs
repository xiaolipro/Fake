using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Fake.EntityFrameworkCore;

public class EfCoreNoRootDbCommandInterceptor : DbCommandInterceptor
{
    public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");

        return base.NonQueryExecuted(command, eventData, result);
    }

    public override ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");

        return base.NonQueryExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override DbCommand CommandInitialized(CommandEndEventData eventData, DbCommand result)
    {
        if (FilterCommand(result.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.CommandInitialized(eventData, result);
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");

        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override ValueTask<InterceptionResult> DataReaderClosingAsync(DbCommand command, DataReaderClosingEventData eventData, InterceptionResult result)
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.DataReaderClosingAsync(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (FilterCommand(command.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
    {
        if (FilterCommand(result.CommandText)) throw new InvalidOperationException($"请不要在无根仓储中执行查询以外的操作！");
        return base.CommandCreated(eventData, result);
    }

    private readonly string[] _filterCommands = new[] { "insert", "update", "drop", "delete", "alter" };

    private bool FilterCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command)) return false;

        var commands = command.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Split(";");

        foreach (var row in commands)
        {
            var line = row.Trim();
            foreach (var cmd in _filterCommands)
            {
                if (line.StartsWith(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }
}