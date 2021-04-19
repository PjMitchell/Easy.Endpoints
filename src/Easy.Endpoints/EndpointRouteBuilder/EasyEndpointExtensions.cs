using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public static class EasyEndpointExtensions
    {
        public static IEndpointConventionBuilder AddEasyEndpoints(
            this IEndpointRouteBuilder endpoints)
        {
            var requestEndPoints = endpoints.ServiceProvider.GetRequiredService<IEndpointManifest>().ToArray();
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
                    BuildDelegate(endPoint.Type),
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

        private static RequestDelegate BuildDelegate(Type type)
        {
            async Task Request(HttpContext context)
            {
                var endpointContext = context.RequestServices.GetRequiredService<EndpointContextAccessor>();
                endpointContext.SetContext(new EndpointContext(context));
                var endpoint = (IEndpoint)context.RequestServices.GetRequiredService(type);
                try
                {
                    await endpoint.HandleRequest(context).ConfigureAwait(false);
                }
                catch(EndpointStatusCodeResponseException e)
                {
                    context.Response.StatusCode = e.StatusCode;
                    await context.Response.WriteAsync(e.Message);
                }
                
            }
            return Request;
        }
    }
}
