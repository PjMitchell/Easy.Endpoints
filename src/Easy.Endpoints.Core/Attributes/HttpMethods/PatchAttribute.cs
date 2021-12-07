namespace Easy.Endpoints
{
    /// <summary>
    /// Defines endpoint with PATCH HttpMethod and optional route information
    /// </summary>
    public class PatchAttribute : EndpointMethodAttribute
    {
        /// <summary>
        /// Creates new instance without route template
        /// </summary>
        public PatchAttribute() : this(null)
        {
        }

        /// <summary>
        /// Creates new instance with route template
        /// </summary>
        /// <param name="template">Route template</param>
        public PatchAttribute(string? template) : base(new[] { "PATCH" }, template)
        {
        }
    }
}
