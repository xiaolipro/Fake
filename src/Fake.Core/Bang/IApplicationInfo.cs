namespace Fake;

public interface IApplicationInfo
{
    [CanBeNull]
    string ApplicationName { get; }
    
    [CanBeNull]
    string ApplicationId { get; }
}