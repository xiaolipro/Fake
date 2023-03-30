using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

public class TestLoggerProvider:ILoggerProvider
{
    ITestOutputHelper _output;

    public TestLoggerProvider(ITestOutputHelper output)
        => _output = output;
    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName) => new TestLogger(categoryName, _output);
}

public class TestLogger : ILogger
{
    string _categoryName;
    ITestOutputHelper _output;

    public TestLogger(string categoryName, ITestOutputHelper output)
    {
        _categoryName = categoryName;
        _output = output;
    }

    public bool IsEnabled(LogLevel logLevel)
        // NB: Only logging things related to commands, but you can easily expand
        //     this
        => _categoryName == DbLoggerCategory.Database.Command.Name;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        // TODO: Customize the formatting even more if you want
        //if (eventId == RelationalEventId.CommandExecuting)
        //{
        //    var structure = (IReadOnlyList<KeyValuePair<string, object>>)state;
        //    var parameters = (string)structure.First(i => i.Key == "parameters")
        //        .Value;
        //    var commandText = (string)structure.First(i => i.Key == "commandText")
        //        .Value;
        //}

        _output.WriteLine(formatter(state, exception));
    }

    public IDisposable BeginScope<TState>(TState state)
        => null;
}