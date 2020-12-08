using System;
using System.Runtime.Serialization;

namespace Beacon.Client.Exceptions
{
    [Serializable]
    public class GetIPException : BeaconServerRequestException
    {
        public GetIPException()
        {
        }

        public GetIPException(string message) : base(message)
        {
        }

        public GetIPException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GetIPException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}