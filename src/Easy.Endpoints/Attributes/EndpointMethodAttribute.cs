using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <summary>
    /// Defines endpoint HttpMethod and optional route information
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class EndpointMethodAttribute : Attribute, IRouteTemplateProvider
    {
        /// <summary>
        /// Creates new instance with httpMethods
        /// </summary>
        /// <param name="httpMethods">Desired Http methods</param>
        protected EndpointMethodAttribute(IEnumerable<string> httpMethods) : this(httpMethods, null)
        {
        }

        /// <summary>
        /// Creates new instance with httpMethods and route template
        /// </summary>
        /// <param name="httpMethods">Desired Http methods</param>
        /// <param name="template">Route template</param>
        protected EndpointMethodAttribute(IEnumerable<string> httpMethods, string? template)
        {
            Template = template;
            HttpMethods = httpMethods;
        }

        /// <summary>
        /// Http methods for Endpoint
        /// </summary>
        public IEnumerable<string> HttpMethods { get; }

        /// <inheritdoc cref="IRouteTemplateProvider.Template"/>
        public string? Template { get; }
        /// <inheritdoc cref="IRouteTemplateProvider.Order"/>
        public int? Order { get; init; }
        /// <inheritdoc cref="IRouteTemplateProvider.Name"/>
        public string? Name { get; init; }
    }
}
