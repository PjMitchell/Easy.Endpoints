namespace Easy.Endpoints
{
    /// <summary>
    /// Parses id part of route as int
    /// </summary>
    public interface IIntIdRouteParser
    {
        /// <summary>
        /// Gets id part of route as an int
        /// </summary>
        /// <returns>id from route</returns>
        /// <exception cref="EndpointStatusCodeResponseException">With as Status code of 404 if cannot parse id</exception>
        int GetIdFromRoute();
    }

    internal class IntIdRouteParser : IIntIdRouteParser
    {
        private readonly IEndpointContextAccessor httpContextAccessor;

        public IntIdRouteParser(IEndpointContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int GetIdFromRoute()
        {
            if (httpContextAccessor.Context is not null && httpContextAccessor.Context.HttpContext.Request.RouteValues.TryGetValue("id", out var value))
            {
                return value switch
                {
                    int i => i,
                    string s => int.Parse(s),
                    _ => 0
                };
            }

            throw new EndpointStatusCodeResponseException(404, "Not found");
        }
    }
}
