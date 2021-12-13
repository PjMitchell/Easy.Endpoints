using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;

namespace Easy.Endpoints
{
    internal static class EndpointParameterInfoFactory
    {
        public static EndpointHandlerParameterDeclaration BuildEndpointParameterDeclaration(ParameterInfo parameterInfo,ICollection<DeclaredRouteParameter> delcaredRouteParameters, EndpointOptions options)
        {

            if (parameterInfo.ParameterType == typeof(CancellationToken))
                return CancellationTokenEndpointParameterInfo.Instance;
            if (parameterInfo.ParameterType == typeof(HttpContext))
                return ContextEndpointParameterInfo.Instance;

            if (parameterInfo.ParameterType == typeof(HttpRequest))
                return RequestEndpointParameterDeclaration.Instance;

            if (parameterInfo.ParameterType == typeof(HttpResponse))
                return ResponseEndpointParameterDeclaration.Instance;

            if (parameterInfo.ParameterType == typeof(ClaimsPrincipal))
                return UserEndpointParameterDeclaration.Instance;

            var parameterBindingSource = parameterInfo.GetCustomAttributes().OfType<IParameterBindingSourceAttribute>().ToArray();

            if (parameterBindingSource.Length > 1)
                throw new InvalidEndpointSetupException($"Endpoint {parameterInfo.Member.DeclaringType?.Name} contains multiple paramter binding source attributes");
            if (parameterBindingSource.Length == 1)
                return ForAttribute(parameterBindingSource[0], parameterInfo, options);


            if (delcaredRouteParameters.Any(r => r.Name == parameterInfo.Name) && ParameterBinder.CanParseRoute(parameterInfo.ParameterType, options) && parameterInfo.Name is not null)
                return ParameterBinder.GetParameterDeclarationForRoute(parameterInfo.Name, parameterInfo.ParameterType, options);

            if (ParameterBinder.CanParseQueryParameter(parameterInfo.ParameterType, options) && parameterInfo.Name is not null)
                return ParameterBinder.GetParameterDeclarationForQuery(parameterInfo.Name, parameterInfo.ParameterType, parameterInfo.HasDefaultValue, parameterInfo.DefaultValue, options);


            return new JsonBodyEndpointParameterDeclaration(parameterInfo.ParameterType, parameterInfo.Name ?? string.Empty);
        }

        private static EndpointHandlerParameterDeclaration ForAttribute(IParameterBindingSourceAttribute attribute, ParameterInfo parameterInfo, EndpointOptions options)
        {
            return attribute switch
            {
                FromBodyAttribute => new JsonBodyEndpointParameterDeclaration(parameterInfo.ParameterType, parameterInfo.Name ?? string.Empty),
                IParameterBindingSourceWithNameAttribute attributeWithName => ParameterBinder.GetParameterDeclarationForBindingAttribute(attributeWithName, parameterInfo.Name ?? string.Empty, parameterInfo.ParameterType, parameterInfo.HasDefaultValue, parameterInfo.DefaultValue, options),
                _ => throw new InvalidEndpointSetupException($"Cannot handle IParameterBindingSourceAttribute of {attribute?.GetType()}")
            };
        }
    }
    
}
