using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Bind from Http body
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromBodyAttribute : Attribute, IParameterBindingSourceAttribute
    {
        /// <inheritdoc cref="IParameterBindingSourceAttribute.Source"/>
        public EndpointParameterSource Source => EndpointParameterSource.Body;
    }
}
