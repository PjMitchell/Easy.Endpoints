namespace Easy.Endpoints
{
    /// <summary>
    /// Defines endpoint with PUT HttpMethod and optional route information
    /// </summary>
    public class PutAttribute : EndpointMethodAttribute
    {
        /// <summary>
        /// Creates new instance without route template
        /// </summary>
        public PutAttribute() : this(null)
        {
        }
        /// <summary>
        /// Creates new instance with route template
        /// </summary>
        /// <param name="template">Route template</param>
        public PutAttribute(string? template) : base(new[] { "PUT" }, template)
        {
        }
    }
}
