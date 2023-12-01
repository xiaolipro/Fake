namespace Fake;

public interface IApplicationInfo
{
    [CanBeNull] string ApplicationName { get; }

    string ApplicationId { get; }
}