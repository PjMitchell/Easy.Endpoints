using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class EndpointMethodAttribute : Attribute, IRouteTemplateProvider
    {
        protected EndpointMethodAttribute(IEnumerable<string> httpMethods) : this(httpMethods, null)
        {
        }

        protected EndpointMethodAttribute(IEnumerable<string> httpMethods, string? template)
        {
            Template = template;
            HttpMethods = httpMethods;
        }

        public IEnumerable<string> HttpMethods { get; }
        public string? Template { get; }
        public int? Order { get; init; }
        public string? Name { get; init; }
    }
}
