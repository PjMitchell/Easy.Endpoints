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
            var context = httpContextAccessor.GetContext();
            if (context.Request.RouteValues.TryGetValue("id", out var value)
                 && value is string stringValue 
                 && int.TryParse(stringValue, out var result)
            )
            {
                return result;
            }

            throw EndpointStatusCodeResponseExceptionHelper.NotFound();
        }
    }
}
