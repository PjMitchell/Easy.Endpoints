using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Easy.Endpoints
{
    /// <summary>
    /// This exception represents an error in resolving the end point route leading to a not found
    /// </summary>
    [Serializable]
    public class EndpointNotFoundException : Exception
    {
        /// <summary>
        /// Creates new instance of EndpointNotFoundException
        /// </summary>
        public EndpointNotFoundException() : base()
        {

        }

        /// <summary>
        /// Creates new instance of EndpointNotFoundException with serialization data
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected EndpointNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// This exception represents an error in resolving the end point parameters
    /// </summary>
    [Serializable]
    public class MalformedRequestException : Exception
    {
        /// <summary>
        /// All binding errors when resolving the end point parameters
        /// </summary>
        public BindingError[] BindingErrors { get; init; } = Array.Empty<BindingError>();

        /// <summary>
        /// Creates new instance of MalformedRequestException
        /// </summary>
        public MalformedRequestException() : base()
        {
            BindingErrors = Array.Empty<BindingError>();
        }

        /// <summary>
        /// Creates new instance of MalformedRequestException
        /// </summary>
        /// <param name="errors">Create new instance of </param>
        public MalformedRequestException(IEnumerable<BindingError> errors)
        {
            BindingErrors = errors.ToArray();
        }

        /// <summary>
        /// Creates new instance of MalformedRequestException with serialization data
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected MalformedRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
