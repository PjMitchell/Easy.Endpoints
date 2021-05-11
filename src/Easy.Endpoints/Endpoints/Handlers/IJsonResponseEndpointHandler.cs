using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that returns a new object
    /// </summary>
    /// <typeparam name="TResponse">Type of Json Response</typeparam>
    public interface IJsonResponseEndpointHandler<TResponse> : IJsonResponse<TResponse>, IEndpointHandler
    {
        /// <summary>
        /// Handles a response and returns a new object
        /// </summary>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>Handled Response object</returns>
        Task<TResponse> HandleAsync(CancellationToken cancellationToken);
    }
}
