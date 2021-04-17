using Microsoft.AspNetCore.Routing.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    public class EndpointInfo
    {
        public EndpointInfo(Type type, RoutePattern pattern, string name, int order) : this(type, null, pattern, name, order)
        {
        }

        public EndpointInfo(Type type, Type? handler, RoutePattern pattern, string name, int order)
        {
            Type = type;
            Handler = handler;
            Pattern = pattern;
            Order = order;
            Name = name;
            Meta = new List<object>();
        }

        public Type Type { get; }
        public Type? Handler { get; }

        public RoutePattern Pattern { get; }
        public string Name { get; }
        public IList<object> Meta { get; }
        public int Order { get; }
    }

    public static class EndpointInfoExtensions
    {
        public static T? GetMetadata<T>(this EndpointInfo source)
        {
            return source.Meta.OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetAllMetadata<T>(this EndpointInfo source)
        {
            return source.Meta.OfType<T>();
        }
    }
}
