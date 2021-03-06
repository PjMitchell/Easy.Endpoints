using System;
using System.Runtime.Serialization;

namespace Easy.Endpoints
{
    /// <summary>
    /// This exception represents an error that will be translated to a status code with message by Easy.Endpoints
    /// </summary>
    [Serializable]
    public class EndpointStatusCodeResponseException : Exception
    {
        /// <summary>
        /// Simple 400 Bad Request
        /// </summary>
        public EndpointStatusCodeResponseException() : this(400, "Bad Request")
        {
        }

        /// <summary>
        /// Exception with status code and message
        /// </summary>
        /// <param name="statusCode">Status for Endpoint response</param>
        /// <param name="message">Message for Endpoint body</param>
        public EndpointStatusCodeResponseException(int statusCode, string message) : this(statusCode, message, null)
        {
        }

        /// <summary>
        /// Exception with status code and message
        /// </summary>
        /// <param name="statusCode">Status for Endpoint response</param>
        /// <param name="message">Message for Endpoint body</param>
        /// <param name="innerException">Inner exception</param>
        public EndpointStatusCodeResponseException(int statusCode, string message, Exception? innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Creates new instance of EndpointStatusCodeResponseException with serialization data
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected EndpointStatusCodeResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Status Code for Endpoint response
        /// </summary>
        public int StatusCode { get; }
    }
}
