using System.Runtime.Serialization;
using System;

namespace Fake.Consul.Exceptions
{
    /// <summary>
    /// Consul未找到服务异常
    /// </summary>
    [Serializable]
    public class ConsulNotFindServiceException : FakeException
    {
        public ConsulNotFindServiceException()
        {
        }

        public ConsulNotFindServiceException(string message) : base(message)
        {
        }

        public ConsulNotFindServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConsulNotFindServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}