using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Endpoints
{
    internal class EasyEndpointApiDescriptionGroupCollectionProvider : IApiDescriptionGroupCollectionProvider
    {
        private readonly IEndpointManifest manifest;
        private readonly ModelMetadataProvider modelMetadataProvider;
        private const string groupName = null;

        public EasyEndpointApiDescriptionGroupCollectionProvider(IEndpointManifest manifest, IEasyEndpointCompositeMetadataDetailsProvider endpointCompositeMetadataDetailsProvider)
        {
            this.manifest = manifest;
            modelMetadataProvider = new DefaultModelMetadataProvider(endpointCompositeMetadataDetailsProvider);
        }

        public ApiDescriptionGroupCollection ApiDescriptionGroups => BuildGroup();

        private ApiDescriptionGroupCollection BuildGroup()
        {
            var apis = new List<ApiDescription>();

            foreach (var endpoint in manifest)
            {
                var actionDescriptor = new ActionDescriptor
                {
                    DisplayName = endpoint.Name,
                    RouteValues = GetRouteValues(endpoint)
                };
                apis.AddRange(GetApiDescriptions(endpoint, actionDescriptor));
            }

            var group = new ApiDescriptionGroup(groupName, apis);

            return new ApiDescriptionGroupCollection(new[] { group }, 1);
        }

        private static IDictionary<string, string?> GetRouteValues(EndpointDeclaration endpoint)
        {
            return endpoint.GetAllMetadata<IEndpointRouteValueMetadataProvider>().ToDictionary<IEndpointRouteValueMetadataProvider, string, string?>(k => k.Key, v => v.Value);
        }

        private IEnumerable<ApiDescription> GetApiDescriptions(EndpointDeclaration endpoint, ActionDescriptor actionDescriptor)
        {
            var httpMethodMetadata = endpoint.GetMetadata<HttpMethodMetadata>();
            if (httpMethodMetadata is null)
                return Enumerable.Empty<ApiDescription>();
            return httpMethodMetadata.HttpMethods.Select(s => GetApiDescriptions(endpoint, s, actionDescriptor));
        }

        private ApiDescription GetApiDescriptions(EndpointDeclaration endpoint, string httpMethod, ActionDescriptor actionDescriptor)
        {
            var apiDescription = new ApiDescription
            {
                GroupName = groupName,
                HttpMethod = httpMethod,
                RelativePath = BuildRoute(endpoint.Pattern),
                ActionDescriptor = actionDescriptor,
            };

            foreach (var response in GetResponseTypes(endpoint))
                apiDescription.SupportedResponseTypes.Add(response);

            ApplyRequestParameters(apiDescription, endpoint);
            return apiDescription;
        }

        private string BuildRoute(RoutePattern routePattern)
        {
            return string.Join("/", routePattern.PathSegments.Select(BuildRouteSegment));
        }

        private string BuildRouteSegment(RoutePatternPathSegment segment)
        {
            var stringBuilder = new StringBuilder();
            foreach (var part in segment.Parts)
            {
                stringBuilder.Append(BuildRoutePart(part));
            }

            return stringBuilder.ToString();
        }

        private static string BuildRoutePart(RoutePatternPart part)
        {
            return part switch
            {
                RoutePatternLiteralPart lit => lit.Content,
                RoutePatternParameterPart param => $"{{{param.Name}}}",
                RoutePatternSeparatorPart sep => sep.Content,
                _ => string.Empty
            };
        }

        private void ApplyRequestParameters(ApiDescription description, EndpointDeclaration endpoint)
        {
            ApplyRequestBodyParameters(description, endpoint);
            foreach (var parameter in endpoint.Pattern.Parameters)
            {
                description.ParameterDescriptions.Add(GetParameterDescriptor(parameter));
            }

            ApplyUrlParameterModelParameters(description, endpoint);
        }

        private void ApplyRequestBodyParameters(ApiDescription description, EndpointDeclaration endpoint)
        {
            var requestBody = endpoint.GetBodyParameterOrDefault();
            if (requestBody is not null)
            {
                var contentTypes = new MediaTypeCollection();
                requestBody.SetContentTypes(contentTypes);
                description.ParameterDescriptions.Add(GetParameterDescriptor(requestBody));

                foreach (var contentType in contentTypes)
                    description.SupportedRequestFormats.Add(new ApiRequestFormat { MediaType = contentType });
            }
        }

        private void ApplyUrlParameterModelParameters(ApiDescription description, EndpointDeclaration endpoint)
        {
            foreach(var data in endpoint.HandlerDeclaration.GetDetails().Where(w=> ParameterHasDescriptor(w) && IsParameterIsNotAlreadyInList(w, description)))
            {
                    description.ParameterDescriptions.Add(GetParameterDescriptor(data));
            }
        }

        private static bool IsParameterIsNotAlreadyInList(EndpointParameterDescriptor info, ApiDescription description) => description.ParameterDescriptions.All(a => a.Name != info.Name);


        private ApiParameterDescription GetParameterDescriptor(RoutePatternParameterPart parameter)
        {
            var type = GetType(parameter);
            return new ApiParameterDescription
            {
                Name = parameter.Name,
                Type = type,
                ModelMetadata = GetModelMetaDataForTypeDefaultingToString(type),
                Source = BindingSource.Path,
                IsRequired = !parameter.IsOptional,
                RouteInfo = new ApiParameterRouteInfo { IsOptional = parameter.IsOptional }
            };
        }

        private ApiParameterDescription GetParameterDescriptor(EndpointParameterDescriptor parameter)
        {
            return new ApiParameterDescription
            {
                Name = parameter.Name,
                Type = parameter.ParameterType,
                ModelMetadata = GetModelMetaDataForTypeDefaultingToString(parameter.ParameterType),
                Source = GetBindingSource(parameter.Source),
                IsRequired = !parameter.IsOptional,
                RouteInfo = new ApiParameterRouteInfo { IsOptional = parameter.IsOptional }
            };
        }

        private ApiParameterDescription GetParameterDescriptor(IEndpointRequestBodyMetadataProvider apiRequestBodyMetadataProvider)
        {
            return new ApiParameterDescription
            {
                Type = apiRequestBodyMetadataProvider.Type,
                ModelMetadata = modelMetadataProvider.GetMetadataForType(apiRequestBodyMetadataProvider.Type),
                Source = BindingSource.Body,
                IsRequired = true
            };
        }

        private ModelMetadata GetModelMetaDataForTypeDefaultingToString(Type type)
        {
            var model = modelMetadataProvider.GetMetadataForType(type);
            if (model.IsComplexType && type != typeof(string))
                return modelMetadataProvider.GetMetadataForType(typeof(string));
            return model;
        }

        private static bool ParameterHasDescriptor(EndpointParameterDescriptor info) => info.Source is EndpointParameterSource.Query or EndpointParameterSource.Header or EndpointParameterSource.Route;
        private static BindingSource GetBindingSource(EndpointParameterSource source)
        {
            return source switch
            {
                EndpointParameterSource.Route => BindingSource.Path,
                EndpointParameterSource.Header => BindingSource.Header,
                EndpointParameterSource.Query => BindingSource.Query,
                _ => throw new NotImplementedException()
            };
        }

        private static Type GetType(RoutePatternParameterPart parameter)
        {
            var type = GetTypeFromParameterPart(parameter);
            return type switch
            {
                "int" => typeof(int),
                "bool" => typeof(bool),
                "datetime" => typeof(DateTime),
                "decimal" => typeof(decimal),
                "double" => typeof(double),
                "float" => typeof(float),
                "guid" => typeof(Guid),
                "long" => typeof(long),
                _ => typeof(string)
            };
        }

        private static string GetTypeFromParameterPart(RoutePatternParameterPart parameter)
        {
            var availableTypes = new HashSet<string>(new[] { "int", "bool", "datetime", "decimal", "double", "float", "guid", "long" });
            var types = parameter.ParameterPolicies.Select(s => s.Content)
                .Where(w => !string.IsNullOrWhiteSpace(w) && availableTypes.Contains(w))
                .ToArray();
            if (types.Length != 1)
                return string.Empty;
            return types[0] ?? string.Empty;
        }

        private IEnumerable<ApiResponseType> GetResponseTypes(EndpointDeclaration endpoint)
        {
            var responses = endpoint.GetApiResponses().Select(s => GetResponseTypes(s));
            return responses;
        }

        private ApiResponseType GetResponseTypes(IApiResponseMetadataProvider meta)
        {
            var contentTypes = new MediaTypeCollection();
            meta.SetContentTypes(contentTypes);
            var result = new ApiResponseType
            {
                StatusCode = meta.StatusCode,
                Type = meta.Type                
            };
            if (meta.Type is not null)
                result.ModelMetadata = modelMetadataProvider.GetMetadataForType(meta.Type);
            result.ApiResponseFormats = contentTypes.Select(s => new ApiResponseFormat { MediaType = s }).ToList();
            return result;
        }
    }
}
