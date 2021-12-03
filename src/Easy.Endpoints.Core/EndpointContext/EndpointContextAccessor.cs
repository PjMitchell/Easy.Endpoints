using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Gets Endpoint Context, accessor is scoped to the request
    /// </summary>
    public interface IEndpointContextAccessor
    {
        /// <summary>
        /// Gets Context
        /// </summary>
        /// <returns>Returns context for request</returns>
        EndpointContext GetContext();
    }

    internal class EndpointContextAccessor : IEndpointContextAccessor
    {
        private EndpointContext? context;

        public EndpointContext SetContext(EndpointContext context) => this.context = context;

        public EndpointContext GetContext()
        {
            if (context is null)
                throw new InvalidOperationException("Context accessor was called before context was set");
            return context;
        }
    }
}
