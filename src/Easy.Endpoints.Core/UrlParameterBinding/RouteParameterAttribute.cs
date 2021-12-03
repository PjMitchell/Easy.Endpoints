using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Declares that a url parameter comes from the route portion
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class RouteParameterAttribute : Attribute
    {
        /// <summary>
        /// Declares that a url parameter comes from the route portion
        /// </summary>
        public RouteParameterAttribute()
        {
        }

        /// <summary>
        /// Declares that a url parameter comes from the route portion with defined name
        /// </summary>
        /// <param name="name">Name of route parameter to be bound to</param>
        public RouteParameterAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of Route Parameter to bind to, if unpopulated it will convert the property name to camelCase
        /// </summary>
        public string? Name { get; }
    }
}
