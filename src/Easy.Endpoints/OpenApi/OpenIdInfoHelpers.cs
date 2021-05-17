using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Easy.Endpoints.GenericTypeHelper;

namespace Easy.Endpoints
{
    internal static class OpenIdInfoHelpers
    {
        public static IEnumerable<IApiResponseMetadataProvider> GetApiResponses(this EndpointInfo info)
        {
            var declaredType = info.GetDeclaredType().GetTypeInfo();
            var jsonResponse = declaredType.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonResponse<>)));
            if (jsonResponse is not null)
                yield return new JsonEndpointResponseMetaData(200, jsonResponse.GenericTypeArguments[0]);
            if (declaredType.ImplementedInterfaces.Any(r => r == typeof(INoContentResponse)))
                yield return new EndpointResponseMetaData(201, typeof(void));
            foreach (var produces in declaredType.GetCustomAttributes<ProducesResponseTypeAttribute>())
            {
                yield return produces.Type == typeof(void) 
                    ? new EndpointResponseMetaData(produces.StatusCode, produces.Type) 
                    : new JsonEndpointResponseMetaData(produces.StatusCode, produces.Type);
            }
                

        }

        public static Type GetDeclaredType(this EndpointInfo info)
        {
            return info.Handler is null ? info.Type : info.Handler;
        }

        public static IEnumerable<UrlParameterMetaData> GetUrlParameterMetaData(this EndpointInfo info)
        {
            var declaredType = info.GetDeclaredType().GetTypeInfo();
            var parameterModel = declaredType.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IUrlParameterModel<>)));
            if (parameterModel is null)
                yield break;
            foreach (var parameter in UrlParameterBindingHelper.GetUrlParameterMetaData(parameterModel.GenericTypeArguments[0]))
                yield return parameter;
        }

        public static IEndpointRequestBodyMetadataProvider? GetBodyParameterOrDefault(this EndpointInfo info)
        {
            var declaredType = info.GetDeclaredType().GetTypeInfo();
            var jsonBody = declaredType.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonBody<>)));
            if (jsonBody is not null)
                return new JsonEndpointRequestBodyMetaData(jsonBody.GenericTypeArguments[0]);
            return null;
        }
    }
}
