using System;
using System.Runtime.Serialization;

namespace Easy.Endpoints
{
    /// <summary>
    /// This exception represents an error in the setup of easy endpoint
    /// </summary>
    [Serializable]
    public class InvalidEndpointSetupException : Exception
    {
        public InvalidEndpointSetupException(string message) : base(message)
        {
        }

        protected InvalidEndpointSetupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
 