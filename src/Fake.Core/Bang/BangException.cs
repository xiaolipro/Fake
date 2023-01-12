using System.Runtime.Serialization;

namespace Fake;

public class FakeException: Exception
{
    public FakeException()
    {

    }

    public FakeException(string message)
        : base(message)
    {

    }

    public FakeException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public FakeException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}