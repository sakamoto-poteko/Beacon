using System;
using System.Runtime.Serialization;

namespace Beacon.Client.Exceptions
{
    public class UpdateIpException : Exception
    {
        public UpdateIpException()
        {
        }

        protected UpdateIpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UpdateIpException(string message) : base(message)
        {
        }

        public UpdateIpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}