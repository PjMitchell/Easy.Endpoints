using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Bind from Http request route
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromRouteAttribute : Attribute, IParameterBindingSourceWithNameAttribute
    {
        /// <summary>
        /// New instance
        /// </summary>
        public FromRouteAttribute()
        {

        }

        /// <summary>
        /// New instance with name
        /// </summary>
        /// <param name="name">Name to bind to</param>
        public FromRouteAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of route parameter to bind to
        /// </summary>
        public string? Name { get; init; }
        /// <inheritdoc cref="IParameterBindingSourceAttribute.Source"/>
        public EndpointParameterSource Source => EndpointParameterSource.Route;
    }
}
