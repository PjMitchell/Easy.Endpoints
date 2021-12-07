using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Bind from Http request query
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromQueryAttribute : Attribute, IParameterBindingSourceWithNameAttribute
    {
        /// <summary>
        /// New instance
        /// </summary>
        public FromQueryAttribute()
        {

        }

        /// <summary>
        /// New instance with name
        /// </summary>
        /// <param name="name">Name to bind to</param>
        public FromQueryAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of query parameter to bind to
        /// </summary>
        public string? Name { get; init; }

        /// <inheritdoc cref="IParameterBindingSourceAttribute.Source" />
        public EndpointParameterSource Source => EndpointParameterSource.Query;
    }
}
