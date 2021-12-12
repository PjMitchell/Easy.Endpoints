using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Easy.Endpoints
{
    internal static class OpenIdInfoHelpers
    {
        public static IEnumerable<IApiResponseMetadataProvider> GetApiResponses(this EndpointInfo info)
        {
            
            if (info.HandlerDeclaration.ReturnType is not null)
            {
                if (info.HandlerDeclaration.ReturnType == typeof(void))
                    yield return new EndpointResponseMetaData(201, typeof(void));
                else if (info.HandlerDeclaration.ReturnType == typeof(string))
                    yield return new PlainTextEndpointResponseMetaData(200);
                else
                    yield return new JsonEndpointResponseMetaData(200, info.HandlerDeclaration.ReturnType);
            }

            foreach (var produces in info.Type.GetCustomAttributes<ProducesResponseTypeAttribute>())
            {
                yield return produces.Type == typeof(void) 
                    ? new EndpointResponseMetaData(produces.StatusCode, produces.Type) 
                    : new JsonEndpointResponseMetaData(produces.StatusCode, produces.Type);
            }
                

        }

        public static IEndpointRequestBodyMetadataProvider? GetBodyParameterOrDefault(this EndpointInfo info)
        {
            var bodyParameter = info.HandlerDeclaration.GetDetails().FirstOrDefault(r => r.Source == EndpointParameterSource.Body);
            if (bodyParameter is not null)
                return new JsonEndpointRequestBodyMetaData(bodyParameter.ParameterType);
            return null;
        }
    }
}
