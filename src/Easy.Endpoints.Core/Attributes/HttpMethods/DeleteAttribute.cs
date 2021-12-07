namespace Easy.Endpoints
{
    /// <summary>
    /// Defines endpoint with DELETE HttpMethod and optional route information
    /// </summary>
    public class DeleteAttribute : EndpointMethodAttribute
    {
        /// <summary>
        /// Creates new instance without route template
        /// </summary>
        public DeleteAttribute() : this(null)
        {
        }

        /// <summary>
        /// Creates new instance with route template
        /// </summary>
        /// <param name="template">Route template</param>
        public DeleteAttribute(string? template) : base(new[] { "DELETE" }, template)
        {
        }
    }
}
