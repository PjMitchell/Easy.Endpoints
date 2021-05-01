using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Adds Easy.Endpoints routes to EndpointRouteBuilder
    /// </summary>
    public static class EndpointConventionBuilderExtensions
    {
        /// <summary>
        /// Adds Easy.Endpoints routes to EndpointRouteBuilder
        /// </summary>
        /// <param name="endpoints">Microsoft.AspNetCore.Routing.IEndpointRouteBuilder to add Easy.Endpoints to</param>
        /// <returns>Returns Microsoft.AspNetCore.Builder.IEndpointConventionBuilder for endpoints</returns>
        [Obsolete("Use MapEasyEndpoints")]
        public static IEndpointConventionBuilder AddEasyEndpoints(
            this IEndpointRouteBuilder endpoints) => MapEasyEndpoints(endpoints);

        /// <summary>
        /// Adds Easy.Endpoints routes to EndpointRouteBuilder
        /// </summary>
        /// <param name="endpoints">Microsoft.AspNetCore.Routing.IEndpointRouteBuilder to add Easy.Endpoints to</param>
        /// <returns>Returns Microsoft.AspNetCore.Builder.IEndpointConventionBuilder for endpoints</returns>
        public static IEndpointConventionBuilder MapEasyEndpoints(
            this IEndpointRouteBuilder endpoints)
        {
            var requestEndPoints = endpoints.ServiceProvider.GetRequiredService<IEndpointManifest>().ToArray();
            var options = endpoints.ServiceProvider.GetRequiredService<EndpointOptions>();
            var dataSource = endpoints.DataSources.OfType<EasyEndpointDataSource>().FirstOrDefault();
            if (dataSource is null)
            {
                dataSource = new EasyEndpointDataSource();
                endpoints.DataSources.Add(dataSource);
            }
            var results = new IEndpointConventionBuilder[requestEndPoints.Length];
            var i = 0;
            foreach (var endPoint in requestEndPoints)
            {
                var builder = new RouteEndpointBuilder(
                    BuildDelegate(endPoint.Type, options),
                    endPoint.Pattern, endPoint.Order)
                {
                    DisplayName = endPoint.Name,
                };
                foreach (var meta in endPoint.Meta)
                    builder.Metadata.Add(meta);
                results[i] = dataSource.AddEndpointBuilder(builder);
                i++;
            }
            return new GroupedEasyEndpointConventionBuilder(results);
        }

        private static RequestDelegate BuildDelegate(Type type, EndpointOptions options)
        {
            async Task Request(HttpContext context)
            {
                var endpointContextAccessor = context.RequestServices.GetRequiredService<EndpointContextAccessor>();
                var endpointContext = new DefaultEndpointContext(context, options);
                endpointContextAccessor.SetContext(endpointContext);
                var endpoint = (IEndpoint)context.RequestServices.GetRequiredService(type);
                try
                {
                    await endpoint.HandleRequest(endpointContext).ConfigureAwait(false);
                }
                catch(EndpointStatusCodeResponseException e)
                {
                    context.Response.StatusCode = e.StatusCode;
                    await context.Response.WriteAsync(e.Message).ConfigureAwait(false);
                }
            }
            return Request;
        }
    }
}
