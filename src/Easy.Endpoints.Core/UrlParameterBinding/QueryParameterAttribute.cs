using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Declares that a url parameter comes from the query portion
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class QueryParameterAttribute : Attribute
    {
        /// <summary>
        /// Declares that a url parameter comes from the query portion with defined name
        /// </summary>
        /// <param name="name">Name of query parameter to be bound to</param>
        public QueryParameterAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of Query Parameter to bind to
        /// </summary>
        public string Name { get; }
    }
}
