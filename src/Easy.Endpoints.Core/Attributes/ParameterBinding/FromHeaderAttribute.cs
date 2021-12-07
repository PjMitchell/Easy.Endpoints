using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Bind from Http request header
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromHeaderAttribute : Attribute, IParameterBindingSourceWithNameAttribute
    {
        /// <summary>
        /// New instance
        /// </summary>
        public FromHeaderAttribute()
        {

        }

        /// <summary>
        /// New instance with name
        /// </summary>
        /// <param name="name">Name to bind to</param>
        public FromHeaderAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of header parameter to bind to
        /// </summary>
        public string? Name { get; init; }

        /// <inheritdoc cref="IParameterBindingSourceAttribute.Source"/>
        public EndpointParameterSource Source => EndpointParameterSource.Header;
    }
}
