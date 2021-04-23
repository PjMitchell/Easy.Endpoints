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
        /// Build Endpoint option
        /// </summary>
        /// <returns>Option Endpoint for builder</returns>
        public EndpointOptions BuildOption() => option;
    }
}
