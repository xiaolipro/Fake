using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fake.EntityFrameworkCore.Interceptors;

public class FakeDbCommandInterceptor: IDbCommandInterceptor
{
    private readonly ICommandFormatter _commandFormatter;

    public FakeDbCommandInterceptor(ICommandFormatter commandFormatter)
    {
        _commandFormatter = commandFormatter;
    }

    public virtual ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        var commandText = command.CommandText;
        var parameters = command.GetParameters();

        Console.WriteLine("------------ReaderExecuting----BEGIN-------------");
        Console.WriteLine(_commandFormatter.Format(commandText, parameters));
        Console.WriteLine("------------ReaderExecuting----END-------------");

        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }


    public virtual ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var commandText = command.CommandText;
        var parameters = command.GetParameters();
        
        Console.WriteLine("------------NonQueryExecuting----BEGIN-------------");
        Console.WriteLine(_commandFormatter.Format(commandText, parameters));
        Console.WriteLine("------------ NonQueryExecuting----END-------------");

        return new ValueTask<InterceptionResult<int>>(result);
    }


    public virtual ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default(CancellationToken))
    {
        var commandText = command.CommandText;
        var parameters = command.GetParameters();

        Console.WriteLine("------------ScalarExecuting----BEGIN-------------");
        Console.WriteLine(_commandFormatter.Format(commandText, parameters));
        Console.WriteLine("------------ ScalarExecuting----END-------------");


        return new ValueTask<InterceptionResult<object>>(result);
    }
}