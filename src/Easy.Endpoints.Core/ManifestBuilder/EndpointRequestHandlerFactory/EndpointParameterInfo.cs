using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Information on the Endpoint's Parameter
    /// </summary>
    public class EndpointParameterInfo
    {
        /// <summary>
        /// Source of Endpoint Parameter, Body, Route etc
        /// </summary>
        public EndpointParameterSource Source { get; init; }

        /// <summary>
        /// Name of parameter
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Parameter Object type
        /// </summary>
        public Type ParameterType { get; init; }

        /// <summary>
        /// If Parameter is optional
        /// </summary>
        public bool IsOptional { get; init; }

        /// <summary>
        /// Factory for building parameter from HttpContext
        /// </summary>
        public ParameterFactory ParameterFactory { get; init; }

        /// <summary>
        /// Creates Info for Parameter
        /// </summary>
        /// <param name="source">Source of parameter</param>
        /// <param name="parameterFactory">Parameter factory for property</param>
        /// <param name="parameterType">Parameter Type</param>
        /// <param name="name">Name of Parameter</param>
        /// <param name="isOptional">If parameter is optional</param>
        /// <returns>New instance of EndpointParameterInfo for parameter</returns>
        public EndpointParameterInfo(EndpointParameterSource source, ParameterFactory parameterFactory, Type parameterType, string name, bool isOptional = false)
        {
            Source = source;
            ParameterFactory = parameterFactory;
            ParameterType = parameterType;  
            Name = name;
            IsOptional = isOptional;
        }

        /// <summary>
        /// Creates Info for route Parameter
        /// </summary>
        /// <param name="parameterFactory">Parameter factory for property</param>
        /// <param name="parameterType">Parameter Type</param>
        /// <param name="name">Name of Parameter</param>
        /// <returns>New instance of EndpointParameterInfo for route parameter</returns>
        public static EndpointParameterInfo Route(ParameterFactory parameterFactory, Type parameterType, string name)
        {
            return new EndpointParameterInfo(EndpointParameterSource.Route, parameterFactory, parameterType, name);
        }

        /// <summary>
        /// Creates Info for query Parameter
        /// </summary>
        /// <param name="parameterFactory">Parameter factory for property</param>
        /// <param name="parameterType">Parameter Type</param>
        /// <param name="name">Name of Parameter</param>
        /// <param name="isOptional">If parameter is optional</param>
        /// <returns>New instance of EndpointParameterInfo for query parameter</returns>
        public static EndpointParameterInfo Query(ParameterFactory parameterFactory, Type parameterType, string name, bool isOptional)
        {
            return new EndpointParameterInfo(EndpointParameterSource.Query, parameterFactory, parameterType, name, isOptional);
        }

        /// <summary>
        /// Creates Info for Parameter that is a predefined type e.g HttpContext
        /// </summary>
        /// <param name="parameterFactory">Parameter factory for property</param>
        /// <param name="parameterType">Parameter Type</param>
        /// <param name="name">Name of Parameter</param>
        /// <returns>New instance of EndpointParameterInfo for predefined parameter</returns>
        public static EndpointParameterInfo Predefined(ParameterFactory parameterFactory, Type parameterType, string name)
        {
            return new EndpointParameterInfo(EndpointParameterSource.Predefined, parameterFactory, parameterType, name);
        }


        /// <summary>
        /// Creates Info for Parameter that is from the http request body
        /// </summary>
        /// <param name="parameterFactory">Parameter factory for property</param>
        /// <param name="parameterType">Parameter Type</param>
        /// <param name="name">Name of Parameter</param>
        /// <returns>New instance of EndpointParameterInfo for body parameter</returns>
        public static EndpointParameterInfo Body(ParameterFactory parameterFactory, Type parameterType, string name)
        {
            return new EndpointParameterInfo(EndpointParameterSource.Body, parameterFactory, parameterType, name);
        }
    }


    /// <summary>
    /// Gets Parameter value from httpContext
    /// </summary>
    /// <param name="httpContext">Source</param>
    /// <param name="options">Endpoint options</param> 
    /// <returns>Value for Parameter</returns>
    public delegate ValueTask<object?> ParameterFactory(HttpContext httpContext, EndpointOptions options);

}
