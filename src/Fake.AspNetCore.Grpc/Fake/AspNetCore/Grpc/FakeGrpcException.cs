using System;
using System.Runtime.Serialization;

namespace Fake.AspNetCore.Grpc;

/// <summary>
/// Grpc发生异常
/// </summary>
public class FakeGrpcException : FakeException
{
    public FakeGrpcException()
    {
    }

    public FakeGrpcException(string message) : base(message)
    {
    }

    public FakeGrpcException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected FakeGrpcException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}