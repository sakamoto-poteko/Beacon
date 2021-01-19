using System;
using System.Runtime.Serialization;

namespace Beacon.Client.Exceptions
{
    [Serializable]
    public class UpdateIPException : BeaconServerRequestException
    {
        public UpdateIPException()
        {
        }

        protected UpdateIPException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UpdateIPException(string message) : base(message)
        {
        }

        public UpdateIPException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}