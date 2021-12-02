using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class EndpointParameterInfoFactory
    {
        public static EndpointParameterInfo BuildEndpointParameterInfo(ParameterInfo parameterInfo,ICollection<DeclaredRouteParameter> delcaredRouteParameters, EndpointOptions options)
        {


            if (parameterInfo.ParameterType == typeof(CancellationToken))
                return EndpointParameterInfo.Predefined(CancelationToken, parameterInfo.ParameterType, parameterInfo.Name ?? string.Empty);

            if (parameterInfo.ParameterType == typeof(HttpContext))
                return EndpointParameterInfo.Predefined(Context, parameterInfo.ParameterType, parameterInfo.Name ?? string.Empty);

            if (parameterInfo.ParameterType == typeof(HttpRequest))
                return EndpointParameterInfo.Predefined(Request, parameterInfo.ParameterType, parameterInfo.Name ?? string.Empty);

            if (parameterInfo.ParameterType == typeof(HttpResponse))
                return EndpointParameterInfo.Predefined(Response, parameterInfo.ParameterType, parameterInfo.Name ?? string.Empty);

            if (parameterInfo.ParameterType == typeof(ClaimsPrincipal))
                return EndpointParameterInfo.Predefined(User, parameterInfo.ParameterType, parameterInfo.Name ?? string.Empty);

            if (delcaredRouteParameters.Any(r => r.Name == parameterInfo.Name) && ParameterBinder.CanParseRoute(parameterInfo.ParameterType) && parameterInfo.Name is not null)
                return ParameterBinder.GetParameterInfoForRoute(parameterInfo.Name, parameterInfo.ParameterType);

            if (ParameterBinder.CanParseQueryParameter(parameterInfo.ParameterType) && parameterInfo.Name is not null)
                return ParameterBinder.GetParameterInfoForQuery(parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.HasDefaultValue, parameterInfo.DefaultValue);


            return EndpointParameterInfo.Body(Body(parameterInfo.ParameterType), parameterInfo.ParameterType, parameterInfo.Name ?? string.Empty);
        }

        private static ValueTask<object?> CancelationToken(HttpContext ctx, EndpointOptions opts) => ValueTask.FromResult<object?>(ctx.RequestAborted);
        private static ValueTask<object?> Context(HttpContext ctx, EndpointOptions opts) => ValueTask.FromResult<object?>(ctx);
        private static ValueTask<object?> User(HttpContext ctx, EndpointOptions opts) => ValueTask.FromResult<object?>(ctx.User);
        private static ValueTask<object?> Response(HttpContext ctx, EndpointOptions opts) => ValueTask.FromResult<object?>(ctx.Response);
        private static ValueTask<object?> Request(HttpContext ctx, EndpointOptions opts) => ValueTask.FromResult<object?>(ctx.Request);

        private static ParameterFactory Body(Type type)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                return HttpContextJsonHelper.ReadJsonBody(ctx, type, opts.JsonSerializerOptions);
            };
        }        
    }
    
}
