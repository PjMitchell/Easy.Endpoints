using System;
using System.Text.Json;

namespace Easy.Endpoints
{
    /// <summary>
    /// Route Pattern when none is specified, defaults "[endpoint]"
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
        /// Build Endpoint option
        /// </summary>
        /// <returns>Option Endpoint for builder</returns>
        public EndpointOptions BuildOption() => option;
    }
}
