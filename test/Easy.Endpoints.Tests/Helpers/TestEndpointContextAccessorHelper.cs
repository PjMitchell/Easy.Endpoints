using Microsoft.AspNetCore.Routing;
using System;

namespace Easy.Endpoints.Tests
{
    public static class TestEndpointContextAccessorHelper
    {
        public static TestEndpointContextAccessor WithIdRouteParameters(this TestEndpointContextAccessor source, string id)
        {
            RouteValueDictionary factory()
            {
                var result = new RouteValueDictionary
                {
                    ["id"] = id
                };
                return result;
            }
            return WithRouteParameters(source, factory);
        }
        public static TestEndpointContextAccessor WithRouteParameters(this TestEndpointContextAccessor source, Func<RouteValueDictionary> routeValueFactory)
        {
            source.Request.SetupGet(s => s.RouteValues).Returns(routeValueFactory);
            return source;
        }
    }

}
