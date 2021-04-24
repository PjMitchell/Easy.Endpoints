﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Easy.Endpoints
{
    internal static class EndpointInfoFactory
    {
        private static readonly string[] get = new[] { "GET" };

        public static EndpointInfo BuildInfoForEndpoint(TypeInfo endpoint, EndpointOptions options)
        {
            return BuildInfoForHandler(null, endpoint, options);
        }

        public static EndpointInfo BuildInfoForHandler(TypeInfo? handler, TypeInfo endpoint, EndpointOptions options)
        {
            var declaredEndpoint = handler ?? endpoint;
            var info = BuildInfoWithRoute(declaredEndpoint, endpoint, handler, options);
            info.MapProducedResponse(declaredEndpoint);
            info.MapBodyParameter(declaredEndpoint);
            return info;
        }

        private static EndpointInfo BuildInfoWithRoute(TypeInfo declaredEndpoint, TypeInfo endpoint, TypeInfo? handler, EndpointOptions options)
        {
            var controllerName = GetControllerValue(declaredEndpoint);
            var endpointValue = GetEndpointValue(declaredEndpoint);
            var routeValues = BuildRouteParameters(controllerName, endpointValue);
            var routeInfo = GetRouteInfo(declaredEndpoint, routeValues, endpointValue.Verb, options);
            var info = new EndpointInfo(endpoint.AsType(),handler is null? null: handler, RoutePatternFactory.Parse(routeInfo.Template), routeInfo.Name, routeInfo.Order ?? 0);
            info.Meta.Add(new HttpMethodMetadata(routeInfo.HttpMethods));
            foreach (var routeValue in routeValues)
                info.Meta.Add(routeValue);
            return info;
        }

        private static string GetControllerValue(TypeInfo handler)
        {
            var metaData = handler.GetCustomAttribute<EndpointControllerAttribute>(true);
            if (metaData is not null)
                return metaData.Name;
            var n = handler.Namespace ?? string.Empty;
            var split = n.Split('.');
            if (split.Length != 0)
                return split[^1];
            return "";
        }

        private static ParsedEndpointName GetEndpointValue(TypeInfo handler)
        {
            var name = handler.Name;
            if(name.EndsWith("EndpointHandler") && name.Length > 15)
                name = name[0..^15];
            else if (name.EndsWith("Endpoint") && name.Length > 8)
                name = name[0..^8];

            return ParsedEndpoint(name);
        }

        private static ParsedEndpointName ParsedEndpoint(string name)
        {
            return name switch
            {
                string v when v.StartsWith("Get") => ParsedEndpoint(v, "GET"),
                string v when v.StartsWith("Post") => ParsedEndpoint(v, "POST"),
                string v when v.StartsWith("Delete") => ParsedEndpoint(v, "DELETE"),
                string v when v.StartsWith("Put") => ParsedEndpoint(v, "PUT"),
                string v when v.StartsWith("Patch") => ParsedEndpoint(v, "PATCH"),
                _ => new ParsedEndpointName(name, null)
            };
        }

        private static ParsedEndpointName ParsedEndpoint(string name, string verb)
        {
            return name.Length == verb.Length ? new ParsedEndpointName(name, verb) : new ParsedEndpointName(name[verb.Length..^0], verb);
        }

        private static void MapProducedResponse(this EndpointInfo info, TypeInfo t)
        {
            var jsonResponse = t.ImplementedInterfaces.SingleOrDefault(r => r.GenericTypeArguments.Length == 1 && r == typeof(IJsonResponse<>).MakeGenericType(r.GenericTypeArguments[0]));
            if (jsonResponse is not null)
                info.Meta.Add(new JsonEndpointResponseMetaData(200, jsonResponse.GenericTypeArguments[0]));
            if (t.ImplementedInterfaces.Any(r => r == typeof(INoContentResponse)))
                info.Meta.Add(new EndpointResponseMetaData(201, typeof(void)));
        }

        private static void MapBodyParameter(this EndpointInfo info, TypeInfo t)
        {
            var ta = t.ImplementedInterfaces.SingleOrDefault(r => r.GenericTypeArguments.Length == 1 && r == typeof(IJsonBody<>).MakeGenericType(r.GenericTypeArguments[0]));
            if (ta is not null)
                info.Meta.Add(new JsonEndpointRequestBodyMetaData(ta.GenericTypeArguments[0]));
        }

        private static ICollection<EndpointRouteValueMetadata> BuildRouteParameters(string controllerName, ParsedEndpointName endpoint)
        {
            return new[]
            {
                new EndpointRouteValueMetadata(EndpointRouteKeys.Controller, controllerName),
                new EndpointRouteValueMetadata(EndpointRouteKeys.Endpoint, endpoint.Name)
            };
        }


        private static EndpointRouteInfo GetRouteInfo(TypeInfo type, ICollection<EndpointRouteValueMetadata> routeValues, string? declaredVerb, EndpointOptions option)
        {
            IEnumerable<string> verbs = get;
            var routeInfo = type.GetCustomAttribute<RouteAttribute>();
            if (routeInfo is not null)
                return BuildFromRouteTemplateProvider(routeInfo, routeValues, verbs);

            var methodInfo = type.GetCustomAttribute<EndpointMethodAttribute>();
            if (methodInfo is not null)
            {
                verbs = methodInfo.HttpMethods;
                if (methodInfo.Template is not null)
                    return BuildFromRouteTemplateProvider(methodInfo, routeValues, verbs);
                else
                    return new EndpointRouteInfo(BuildPatternFromRouteValues(option.RoutePattern, routeValues), BuildName(routeValues), 0, verbs);
            }
            return new EndpointRouteInfo(BuildPatternFromRouteValues(option.RoutePattern, routeValues), BuildName(routeValues), 0, string.IsNullOrWhiteSpace(declaredVerb) ? verbs : new[] { declaredVerb });
        }

        private static EndpointRouteInfo BuildFromRouteTemplateProvider(IRouteTemplateProvider routeInfo, ICollection<EndpointRouteValueMetadata> routeValues, IEnumerable<string> verbs)
        {
            return new EndpointRouteInfo(
                BuildPatternFromRouteValues(routeInfo.Template, routeValues),
                string.IsNullOrWhiteSpace(routeInfo.Name)? BuildName(routeValues): routeInfo.Name,
                routeInfo.Order, 
                verbs);
        }

        private static string BuildPatternFromRouteValues(string pattern, ICollection<EndpointRouteValueMetadata> routeValues)
        {
            var result = pattern;
            foreach(var routeValue in routeValues)
                result = pattern.Replace($"[{routeValue.Key}]", routeValue.Value);
            return result;
        }

        private static string BuildName(ICollection<EndpointRouteValueMetadata> routeValues)
        {
            return routeValues.Single(s=> s.Key == "endpoint").Value;
        }

        private class EndpointRouteInfo : IRouteTemplateProvider
        {
            public EndpointRouteInfo(IRouteTemplateProvider routeTemplate, IEnumerable<string> methods) : this(routeTemplate.Template, routeTemplate.Name, routeTemplate.Order, methods)
            {
            }

            public EndpointRouteInfo(string template, string name, int? order, IEnumerable<string> methods)
            {
                Template = template;
                Name = name;
                Order = order;
                HttpMethods = methods;
            }

            public string Template { get; }
            public int? Order { get; }
            public string Name { get; }
            public IEnumerable<string> HttpMethods { get; }
        }

        public record ParsedEndpointName(string Name, string? Verb)
        {

        }
    }
}