using System.Text;
using Fake.AspNetCore.Localization;
using Fake.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Fake.AspNetCore.ExceptionHandling;

public interface IExceptionToErrorInfoConverter
{
    RemoteServiceErrorInfo Convert(Exception exception, FakeExceptionHandlingOptions options);
}

public class DefaultExceptionToErrorInfoConverter(IStringLocalizer<FakeAspNetCoreResource> localizer)
    : IExceptionToErrorInfoConverter
{
    public virtual RemoteServiceErrorInfo Convert(Exception exception, FakeExceptionHandlingOptions options)
    {
        var errorModel = new RemoteServiceErrorInfo
        {
            Message = exception.Message
        };

        if (exception is IHasErrorCode hasErrorCodeException)
        {
            errorModel.Code = hasErrorCodeException.Code;
            if (!hasErrorCodeException.Code.IsNullOrWhiteSpace())
            {
                errorModel.Message = localizer[errorModel.Code!];
            }
        }

        if (options.OutputStackTrace)
        {
            var stackTrace = new StringBuilder();
            ExceptionStackTrace(exception, stackTrace);
            errorModel.StackTrace = stackTrace.ToString();
        }

        return errorModel;
    }

    protected virtual void ExceptionStackTrace(Exception exception, StringBuilder stackTrace)
    {
        // message
        stackTrace.AppendLine(exception.GetType().Name + ": " + exception.Message);

        // stack trace
        stackTrace.AppendLine($"Stack Trace: {exception.StackTrace}");

        // inner exception
        if (exception.InnerException != null)
        {
            ExceptionStackTrace(exception.InnerException, stackTrace);
        }

        // aggregate exception
        if (exception is AggregateException aggregateException)
        {
            foreach (var innerException in aggregateException.InnerExceptions)
            {
                ExceptionStackTrace(innerException, stackTrace);
            }
        }
    }
}