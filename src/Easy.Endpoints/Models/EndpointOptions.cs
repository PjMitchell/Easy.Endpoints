namespace Easy.Endpoints
{
    /// <summary>
    /// Option for Endpoints
    /// </summary>
    public class EndpointOptions
    {
        /// <summary>
        /// Route Pattern when none is specified, defaults "[endpoint]"
        /// </summary>
        public string RoutePattern { get; internal set; } = "[endpoint]";
    }
}
