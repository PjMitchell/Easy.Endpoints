namespace Easy.Endpoints
{
    /// <summary>
    /// Defines endpoint with POST HttpMethod and optional route information
    /// </summary>
    public class PostAttribute : EndpointMethodAttribute
    {
        /// <summary>
        /// Creates new instance without route template
        /// </summary>
        public PostAttribute() : this(null)
        {
        }

        /// <summary>
        /// Creates new instance with route template
        /// </summary>
        /// <param name="template">Route template</param>
        public PostAttribute(string? template) : base(new[] { "POST" }, template)
        {
        }
    }
}
