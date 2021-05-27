using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Easy.Endpoints
{
    /// <summary>
    /// Used to construct the endpoint options
    /// </summary>
    public class EndpointOptionBuilders
    {
        private readonly EndpointOptions option;

        /// <summary>
        /// Constructs new EndpointOptionBuilders
        /// </summary>
        public EndpointOptionBuilders()
        {
            option = new EndpointOptions();
        }

        /// <summary>
        /// Defines Route Pattern when no route is defined for an endpoint
        /// </summary>
        /// <param name="routePattern">new route pattern for endpoint</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithRoutePattern(string routePattern)
        {
            option.RoutePattern = routePattern;
            return this;
        }

        /// <summary>
        /// Adds Json Serializer Options, if not defined default settings will be used
        /// </summary>
        /// <param name="jsonSerializerOptions">jsonSerializerOptions to be used by endpoint</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            option.JsonSerializerOptions = jsonSerializerOptions;
            return this;
        }

        /// <summary>
        /// Modifies default JsonSerializerOptions for Endpoints
        /// </summary>
        /// <param name="jsonSerializerOptionsModification">Action that modifies JsonSerializerOptions</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithJsonSerializer(Action<JsonSerializerOptions> jsonSerializerOptionsModification)
        {
            jsonSerializerOptionsModification(option.JsonSerializerOptions);
            return this;
        }

        /// <summary>
        /// Adds all IEndpointMetaDataDeclaration to builder
        /// </summary>
        /// <param name="declarations">All IEndpointMetaDataDeclaration to be added</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithMetaDataDeclarations(IEnumerable<IEndpointMetaDataDeclaration> declarations)
        {
            option.EndpointMetaDeclarations = declarations.ToArray();
            return this;
        }

        /// <summary>
        /// Adds all IEndpointMetaDataDeclaration to builder
        /// </summary>
        /// <param name="declarationsModification">Modification of existing IEndpointMetaDataDeclarations</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithMetaDataDeclarations(Func<IEnumerable<IEndpointMetaDataDeclaration>, IEnumerable<IEndpointMetaDataDeclaration>> declarationsModification)
        {
            option.EndpointMetaDeclarations = declarationsModification(option.EndpointMetaDeclarations).ToArray();
            return this;
        }

        /// <summary>
        /// Adds all IEndpointForHandlerDeclaration to builder
        /// </summary>
        /// <param name="declarations">All IEndpointForHandlerDeclaration to be added</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithEndpointForHandlerDeclarations(IEnumerable<IEndpointForHandlerDeclaration> declarations)
        {
            option.EndpointForHandlerDeclarations = declarations.ToArray();
            return this;
        }

        /// <summary>
        /// Adds all IEndpointForHandlerDeclaration to builder
        /// </summary>
        /// <param name="declarationsModification">Modification of existing IEndpointForHandlerDeclaration</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithEndpointForHandlerDeclarations(Func<IEnumerable<IEndpointForHandlerDeclaration>, IEnumerable<IEndpointForHandlerDeclaration>> declarationsModification)
        {
            option.EndpointForHandlerDeclarations = declarationsModification(option.EndpointForHandlerDeclarations).ToArray();
            return this;
        }

        /// <summary>
        /// Build Endpoint option
        /// </summary>
        /// <returns>Option Endpoint for builder</returns>
        public EndpointOptions BuildOption() => option;
    }
}
