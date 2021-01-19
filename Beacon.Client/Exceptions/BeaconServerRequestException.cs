using System;
using System.Runtime.Serialization;

namespace Beacon.Client.Exceptions
{
    [Serializable]
    public class BeaconServerRequestException : Exception
    {
        public BeaconServerRequestException() { }
        public BeaconServerRequestException(string message) : base(message) { }
        public BeaconServerRequestException(string message, Exception inner) : base(message, inner) { }
        protected BeaconServerRequestException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}