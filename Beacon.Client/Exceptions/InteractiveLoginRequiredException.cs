using System;
using System.Runtime.Serialization;

namespace Beacon.Client.Exceptions
{
    [Serializable]
    internal class InteractiveLoginRequiredException : AuthorizationTokenException
    {
        public InteractiveLoginRequiredException()
        {
        }

        public InteractiveLoginRequiredException(string message) : base(message)
        {
        }

        public InteractiveLoginRequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InteractiveLoginRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}