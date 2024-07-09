using System.Text;
using Fake.ExceptionHandling;
using Fake.Localization;

namespace Fake.AspNetCore.ExceptionHandling;

public interface IExceptionToErrorInfoConverter
{
    RemoteServiceErrorInfo Convert(Exception exception, FakeExceptionHandlingOptions options);
}

public class DefaultExceptionToErrorInfoConverter(IFakeStringLocalizerFactory localizerFactory)
    : IExceptionToErrorInfoConverter
{
    public virtual RemoteServiceErrorInfo Convert(Exception exception, FakeExceptionHandlingOptions options)
    {
        var errorInfo = new RemoteServiceErrorInfo
        {
            Message = exception.Message
        };

        if (exception is IHasErrorCode hasErrorCodeException)
        {
            errorInfo.Code = hasErrorCodeException.Code;
            TryLocalizeExceptionMessage(hasErrorCodeException, errorInfo);
        }

        if (options.OutputStackTrace)
        {
            var stackTrace = new StringBuilder();
            ExceptionStackTrace(exception, stackTrace);
            errorInfo.StackTrace = stackTrace.ToString();
        }

        return errorInfo;
    }

    private void TryLocalizeExceptionMessage(IHasErrorCode hasErrorCodeException, RemoteServiceErrorInfo errorInfo)
    {
        if (hasErrorCodeException.Code.IsNullOrWhiteSpace()) return;
        if (!hasErrorCodeException.Code!.Contains(':')) return;

        var resourceName = hasErrorCodeException.Code.Split(':')[0];
        var localizer = localizerFactory.CreateByResourceNameOrNull(resourceName);
        if (localizer == null) return;

        var localizedString = localizer[errorInfo.Code!];
        if (localizedString.ResourceNotFound) return;
        if (localizedString.Value.IsNullOrWhiteSpace()) return;

        errorInfo.Message = localizedString.Value;
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