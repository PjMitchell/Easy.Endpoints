using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Parses id part of route as int
    /// </summary>
    public interface IGuidIdRouteParser
    {
        /// <summary>
        /// Gets id part of route as an Guid
        /// </summary>
        /// <returns>id from route</returns>
        /// <exception cref="EndpointStatusCodeResponseException">With as Status code of 404 if cannot parse id</exception>
        Guid GetIdFromRoute();
    }

    internal class GuidIdRouteParser : IGuidIdRouteParser
    {
        private readonly IEndpointContextAccessor httpContextAccessor;

        public GuidIdRouteParser(IEndpointContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid GetIdFromRoute()
        {
            var context = httpContextAccessor.GetContext();
            if (context.Request.TryGetRouteParameter("id", out Guid result))
            {
                return result;
            }

            throw EndpointStatusCodeResponseExceptionHelper.NotFound();
        }
    }
}
