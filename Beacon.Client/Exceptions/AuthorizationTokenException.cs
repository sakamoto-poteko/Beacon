using System;
using System.Runtime.Serialization;
using Microsoft.Identity.Client;

namespace Beacon.Client.Exceptions
{
    [Serializable]
    public class AuthorizationTokenException : Exception
    {
        private MsalException exception;

        public AuthorizationTokenException()
        {
        }

        public AuthorizationTokenException(MsalException exception)
        {
            this.exception = exception;
        }

        public AuthorizationTokenException(string message) : base(message)
        {
        }

        public AuthorizationTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AuthorizationTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}