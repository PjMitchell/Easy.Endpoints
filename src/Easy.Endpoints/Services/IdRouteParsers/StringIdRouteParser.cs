namespace Easy.Endpoints
{
    /// <summary>
    /// Parses id part of route as string
    /// </summary>
    public interface IStringIdRouteParser
    {
        /// <summary>
        /// Gets id part of route as a string
        /// </summary>
        /// <returns>id from route</returns>
        /// <exception cref="EndpointStatusCodeResponseException">With as Status code of 404 if cannot parse id</exception>
        string GetIdFromRoute();
    }
    internal class StringIdRouteParser : IStringIdRouteParser
    {
        private readonly IEndpointContextAccessor httpContextAccessor;

        public StringIdRouteParser(IEndpointContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetIdFromRoute()
        {
            var context = httpContextAccessor.GetContext();
            if (context.Request.RouteValues.TryGetValue("id", out var value)
                 && value is string stringValue
            )
            {
                return stringValue;
            }

            throw EndpointStatusCodeResponseExceptionHelper.NotFound();
        }
    }
}
