namespace Easy.Endpoints
{
    /// <summary>
    /// Defines endpoint with GET HttpMethod and optional route information
    /// </summary>
    public class GetAttribute : EndpointMethodAttribute
    {
        /// <summary>
        /// Creates new instance without route template
        /// </summary>
        public GetAttribute() : this(null)
        {
        }

        /// <summary>
        /// Creates new instance with route template
        /// </summary>
        /// <param name="template">Route template</param>
        public GetAttribute(string? template) : base(new[] { "GET" }, template)
        {
        }
    }
}
