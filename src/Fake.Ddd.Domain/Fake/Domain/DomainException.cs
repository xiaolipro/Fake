namespace Fake.Domain;

/// <summary>
/// 领域异常
/// </summary>
/// <remarks>
/// 专注于领域内发生的异常，隶属于<exception cref="BusinessException">业务异常</exception>
/// </remarks>
public class DomainException : BusinessException
{
    public DomainException()
    {
    }

    public DomainException(string message)
        : base(message)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException: innerException)
    {
    }
}