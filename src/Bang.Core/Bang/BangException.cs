using System.Runtime.Serialization;

namespace Bang;

public class BangException: Exception
{
    public BangException()
    {

    }

    public BangException(string message)
        : base(message)
    {

    }

    public BangException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public BangException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}