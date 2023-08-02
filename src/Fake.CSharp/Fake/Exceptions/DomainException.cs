namespace Fake.CSharp.Fake.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {

    }
}


public class DefaultDomainException : DomainException
{
    public DefaultDomainException(string message) : base(message)
    {
    }
}