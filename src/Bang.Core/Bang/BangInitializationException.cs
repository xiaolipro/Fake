using System.Runtime.Serialization;

namespace Bang;

public class BangInitializationException:BangException
{
    public BangInitializationException()
    {

    }

    public BangInitializationException(string message)
        : base(message)
    {

    }

    public BangInitializationException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public BangInitializationException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}