namespace Easy.Endpoints
{
    /// <summary>
    /// Source of Endpoint Parameter
    /// </summary>
    public enum EndpointParameterSource
    {
        /// <summary>
        /// Used if not found
        /// </summary>
        Unknown,
        /// <summary>
        /// Predefined Types such as HttpContext, HttpResponse, etc.
        /// </summary>
        Predefined,
        /// <summary>
        /// From Http Body
        /// </summary>
        Body,
        /// <summary>
        /// From Url Route
        /// </summary>
        Route,
        /// <summary>
        /// From Url Query param
        /// </summary>
        Query
    }

}
