namespace Fake;

public interface IApplicationInfo
{
    [CanBeNull]
    string ApplicationName { get; }
    
    [NotNull]
    string ApplicationId { get; }
}