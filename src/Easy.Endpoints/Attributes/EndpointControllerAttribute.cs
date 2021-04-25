using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Defines Controller Route Value for an endpoint, useful for grouping endpoints
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EndpointControllerAttribute : Attribute
    {
        /// <summary>
        /// Creates new instance of EndpointControllerAttribute
        /// </summary>
        /// <param name="name">Name of controller</param>
        public EndpointControllerAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of controller
        /// </summary>
        public string Name { get;  }
    }
}
