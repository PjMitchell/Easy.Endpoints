using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    internal static class EndpointInfoExtensions
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
