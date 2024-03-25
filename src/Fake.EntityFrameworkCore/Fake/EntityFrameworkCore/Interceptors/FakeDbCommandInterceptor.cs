using System.Data.Common;
using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Fake.EntityFrameworkCore.Interceptors;

public class FakeDbCommandInterceptor : IDbCommandInterceptor
{
    private readonly ICommandFormatter _commandFormatter;
    private readonly ILogger<FakeDbCommandInterceptor> _logger;
    private Stopwatch _stopwatch = default!;

    public FakeDbCommandInterceptor(ICommandFormatter commandFormatter, ILogger<FakeDbCommandInterceptor> logger)
    {
        _commandFormatter = commandFormatter;
        _logger = logger;
    }

    public virtual ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        _stopwatch = Stopwatch.StartNew();

        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    public ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _stopwatch.Stop();
        var sb = new StringBuilder();

        sb.AppendLine($"DbCommand LOG: [ReaderExecuting]");
        // sb.AppendLine($"- UserName - Id      : {UserName} - {Id}");
        // sb.AppendLine($"- ClientIpAddress        : {ClientIpAddress}");
        sb.AppendLine($"- ExecutionDuration      : {_stopwatch.ElapsedMilliseconds} ms");
        sb.AppendLine($"  {_commandFormatter.Format(command)}");

        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.LogInformation(sb.ToString());

        return new ValueTask<DbDataReader>(result);
    }


    public virtual ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command,
        CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("------------NonQueryExecuting----BEGIN-------------");
        Console.WriteLine(_commandFormatter.Format(command));
        Console.WriteLine("------------ NonQueryExecuting----END-------------");

        return new ValueTask<InterceptionResult<int>>(result);
    }


    public virtual ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command,
        CommandEventData eventData, InterceptionResult<object> result,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        Console.WriteLine("------------ScalarExecuting----BEGIN-------------");
        Console.WriteLine(_commandFormatter.Format(command));
        Console.WriteLine("------------ ScalarExecuting----END-------------");


        return new ValueTask<InterceptionResult<object>>(result);
    }
}