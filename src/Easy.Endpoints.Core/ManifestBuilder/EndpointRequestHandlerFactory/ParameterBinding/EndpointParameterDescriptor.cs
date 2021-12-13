using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Information for Parameter
    /// </summary>
    public class EndpointParameterDescriptor
    {
        /// <summary>
        /// Source of Endpoint Parameter, Body, Route etc
        /// </summary>
        public EndpointParameterSource Source { get; }

        /// <summary>
        /// Name of parameter
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parameter Object type
        /// </summary>
        public Type ParameterType { get; }

        /// <summary>
        /// If Parameter is optional
        /// </summary>
        public bool IsOptional { get; }

        /// <summary>
        /// Creates new instance of EndpointParameterDescriptor
        /// </summary>
        /// <param name="source">Source of Endpoint Parameter, Body, Route etc</param>
        /// <param name="parameterType">Parameter Object type</param>
        /// <param name="name">Name of parameter</param>
        /// <param name="isOptional">If Parameter is optional</param>
        public EndpointParameterDescriptor(EndpointParameterSource source, Type parameterType, string name, bool isOptional = false)
        {
            Source = source;
            ParameterType = parameterType;
            Name = name;
            IsOptional = isOptional;
        }
    }

}
