using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace Easy.Endpoints
{
    public static class EndpointInfoFactory
    {
        private static readonly string[] get = new[] { "GET" };

        public static EndpointInfo BuildInfoForEndpoint(TypeInfo endpoint)
        {
            return BuildInfo(endpoint, e => BuildBase(e, endpoint));
        }

        public static EndpointInfo BuildInfoForHandler(TypeInfo handler, TypeInfo endpoint)
        {
            return BuildInfo(handler, e => BuildBase(e, handler, endpoint));
        }

        public static EndpointInfo BuildInfo(TypeInfo endpoint, Func<ParsedEndpointName, EndpointInfo> endpointFactory)
        {
            var controllerName = GetControllerValue(endpoint);
            var endpointValue = GetEndpointValue(endpoint);
            var info = endpointFactory(endpointValue);
            info.MapProducedResponse(endpoint);
            info.MapBodyParameter(endpoint);
            info.MapRouteParameters(controllerName, endpointValue);
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

        private static void MapRouteParameters(this EndpointInfo info, string controllerName, ParsedEndpointName endpoint)
        {
            info.Meta.Add(new EndpointRouteValueMetadata(EndpointRouteKeys.Controller, controllerName));
            info.Meta.Add(new EndpointRouteValueMetadata(EndpointRouteKeys.Endpoint, endpoint.Name));

        }

        private static EndpointInfo BuildBase(ParsedEndpointName parsedEndpointName, TypeInfo handler, TypeInfo endpoint)
        {
            var routeInfo = GetRouteInfo(parsedEndpointName, handler);
            return BuildBase(handler, endpoint, routeInfo);
        }
        private static EndpointInfo BuildBase(ParsedEndpointName parsedEndpointName, TypeInfo type)
        {
            var routeInfo = GetRouteInfo(parsedEndpointName, type);
            return BuildBase(type, routeInfo);
        }

        private static EndpointInfo BuildBase(TypeInfo type, EndpointRouteInfo routeInfo)
        {
            var info = new EndpointInfo(type.AsType(), RoutePatternFactory.Parse(routeInfo.Template), routeInfo.Name, routeInfo.Order ?? 0);
            info.Meta.Add(new HttpMethodMetadata(routeInfo.HttpMethods));
            return info;
        }

        private static EndpointInfo BuildBase(TypeInfo handler, TypeInfo endpoint, EndpointRouteInfo routeInfo)
        {
            var info = new EndpointInfo(endpoint.AsType(), handler.AsType(), RoutePatternFactory.Parse(routeInfo.Template), routeInfo.Name, routeInfo.Order ?? 0);
            info.Meta.Add(new HttpMethodMetadata(routeInfo.HttpMethods));
            return info;
        }

        private static EndpointRouteInfo GetRouteInfo(ParsedEndpointName parsedEndpointName, TypeInfo type)
        {
            IEnumerable<string> verbs = get;
            var routeInfo = type.GetCustomAttribute<RouteAttribute>();
            if (routeInfo is not null)
                return new EndpointRouteInfo(routeInfo, verbs);

            var methodInfo = type.GetCustomAttribute<EndpointMethodAttribute>();
            if (methodInfo is not null)
            {
                verbs = methodInfo.HttpMethods;
                if (methodInfo.Template is not null)
                    return new EndpointRouteInfo(methodInfo, verbs);
                else
                    return new EndpointRouteInfo(parsedEndpointName.Name, parsedEndpointName.Name,0, verbs);
            }
            return new EndpointRouteInfo(parsedEndpointName.Name, parsedEndpointName.Name, 0, string.IsNullOrWhiteSpace(parsedEndpointName.Verb)? verbs : new[] { parsedEndpointName.Verb });
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
