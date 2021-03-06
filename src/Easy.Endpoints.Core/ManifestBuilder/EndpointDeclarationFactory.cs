using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace Easy.Endpoints
{
    internal static class EndpointDeclarationFactory
    {
        private static readonly string[] get = new[] { "GET" };

        public static EndpointDeclaration BuildDeclarationForEndpoint(TypeInfo endpoint, EndpointDeclarationFactoryOptions options, params object[] meta)
        {
            var declaredEndpoint = endpoint;
            var info = BuildInfoWithRoute(declaredEndpoint, options, meta.OfType<EndpointRouteValueMetadata>());
            info.MapAuthMeta(declaredEndpoint);
            AddMeta(info, options, declaredEndpoint, meta);
            return info;
        }

        private static void AddMeta(EndpointDeclaration info, EndpointDeclarationFactoryOptions options, TypeInfo declaredEndpoint, object[] meta)
        {
            var allMeta = options.EndpointMetaDeclarations
                .SelectMany(declaration => declaration.GetMetaDataFromDeclaredEndpoint(declaredEndpoint))
                .Concat(meta);
            foreach (var item in allMeta)
                info.Meta.Add(item);
        }

        private static void MapAuthMeta(this EndpointDeclaration info, TypeInfo declaredEndpoint)
        {
            var attributesToMap = declaredEndpoint.GetCustomAttributes().Where(t => t is IAuthorizeData || t is IAllowAnonymous);
            foreach (var attribute in attributesToMap)
                info.Meta.Add(attribute);
        }

        private static EndpointDeclaration BuildInfoWithRoute(TypeInfo endpoint, EndpointDeclarationFactoryOptions options, IEnumerable<EndpointRouteValueMetadata> routeValueMetaData)
        {
            var controllerName = GetControllerValue(endpoint);
            var endpointValue = GetEndpointValue(endpoint);
            var routeValues = BuildRouteParameters(controllerName, endpointValue);
            var routeInfo = GetRouteInfo(endpoint, routeValues.Concat(routeValueMetaData).ToArray(), endpointValue.Verb, options);
            var declaredRouteInfo = DeclaredRouteInfoFactory.GetFromTemplate(routeInfo.Template);
            var info = new EndpointDeclaration(endpoint.AsType(), EndpointRequestHandlerFactoryBuilder.BuildFactoryForEndpoint(endpoint,declaredRouteInfo, options), RoutePatternFactory.Parse(routeInfo.Template), routeInfo.Name, routeInfo.Order ?? 0);
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

        private static ICollection<EndpointRouteValueMetadata> BuildRouteParameters(string controllerName, ParsedEndpointName endpoint)
        {
            return new[]
            {
                new EndpointRouteValueMetadata(EndpointRouteKeys.Controller, controllerName),
                new EndpointRouteValueMetadata(EndpointRouteKeys.Endpoint, endpoint.Name)
            };
        }

        private static EndpointRouteInfo GetRouteInfo(TypeInfo type, ICollection<EndpointRouteValueMetadata> routeValues, string? declaredVerb, EndpointDeclarationFactoryOptions option)
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
                BuildPatternFromRouteValues(routeInfo.Template ?? "", routeValues),
                string.IsNullOrWhiteSpace(routeInfo.Name)? BuildName(routeValues): routeInfo.Name,
                routeInfo.Order,
                verbs);
        }

        private static string BuildPatternFromRouteValues(string pattern, ICollection<EndpointRouteValueMetadata> routeValues)
        {
            foreach(var routeValue in routeValues)
                pattern = pattern.Replace($"[{routeValue.Key}]", routeValue.Value);
            return pattern;
        }

        private static string BuildName(ICollection<EndpointRouteValueMetadata> routeValues)
        {
            return routeValues.Single(s=> s.Key == "endpoint").Value;
        }

        

        private sealed class EndpointRouteInfo : IRouteTemplateProvider
        {
            public EndpointRouteInfo(IRouteTemplateProvider routeTemplate, IEnumerable<string> methods) : this(routeTemplate.Template ?? string.Empty, routeTemplate.Name ?? string.Empty, routeTemplate.Order, methods)
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

        internal static class DeclaredRouteInfoFactory
        {
            public static DeclaredRouteInfo GetFromTemplate(string template)
            {
                var parameters = Regex.Matches(template, @"\{([^{}]+)\}*")
                    .Where(w => w.Groups.Count == 2)
                    .Select(match => {
                    var values = match.Groups[1].Value.Split(':');
                    return new DeclaredRouteParameter
                    {
                        Name = values[0].Trim('*'),
                    };
                }).ToArray();
                return new DeclaredRouteInfo
                {
                    Parameters = parameters,
                };
                
            }
        }
    }
}
