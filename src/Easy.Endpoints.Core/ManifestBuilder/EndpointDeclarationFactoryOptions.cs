using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    internal class EndpointDeclarationFactoryOptions
    {
        public EndpointDeclarationFactoryOptions(string routePatern, IEnumerable<IEndpointMetaDataDeclaration> endpointMetaDeclarations, IParserCollection parsers)
        {
            RoutePattern = routePatern;
            EndpointMetaDeclarations = endpointMetaDeclarations.ToArray();
            Parsers = parsers;
        }

        public string RoutePattern { get; } 

        public IEndpointMetaDataDeclaration[] EndpointMetaDeclarations { get; }

        public IParserCollection Parsers { get; }
    }

    internal class EasyEndpointBuilderOptions
    {
        public string RoutePattern { get; set; } = "[endpoint]";
    }
}
