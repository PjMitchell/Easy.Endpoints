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
        /// <summary>
        /// Creates new instance of InvalidEndpointSetupException with message
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidEndpointSetupException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates new instance of InvalidEndpointSetupException with serialization data
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected InvalidEndpointSetupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
