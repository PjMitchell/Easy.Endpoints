using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    /// <summary>
    /// Handler that updates the http context for a MalformedRequestException
    /// </summary>
    public interface IMalformedRequestExceptionHandler
    {
        /// <summary>
        /// Delgate that updates the http context for a MalformedRequestException
        /// </summary>
        /// <param name="ex">Malformed Request Exception to be handled</param>
        /// <param name="httpContext">Current request context</param>
        /// <returns>Task for the operation</returns>
        Task HandleMalformedRequest(MalformedRequestException ex, HttpContext httpContext);
    }

    internal class DefaultIMalformedRequestExceptionHandler : IMalformedRequestExceptionHandler
    {
        public Task HandleMalformedRequest(MalformedRequestException ex, HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 400;
            return httpContext.Response.WriteAsync("Bad Request");
        }
    }
}
