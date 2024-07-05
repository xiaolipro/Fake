namespace Fake.Data;

public class FakeDbConcurrencyException : FakeException
{
    /// <summary>
    /// Creates a new <see cref="FakeDbConcurrencyException"/> object.
    /// </summary>
    public FakeDbConcurrencyException()
    {
    }


    /// <summary>
    /// Creates a new <see cref="FakeDbConcurrencyException"/> object.
    /// </summary>
    /// <param name="innerException">Exception message</param>
    public FakeDbConcurrencyException(Exception? innerException)
        : base(null, innerException)
    {
    }

    /// <summary>
    /// Creates a new <see cref="FakeDbConcurrencyException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    public FakeDbConcurrencyException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new <see cref="FakeDbConcurrencyException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public FakeDbConcurrencyException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}