using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that processes a json body and returns a new object
    /// </summary>
    /// <typeparam name="TBody">Type of Json Body</typeparam>
    /// <typeparam name="TResponse">Type of Json Response</typeparam>
    public interface IJsonBodyAndResponseEndpointHandler<TBody, TResponse> : IJsonBody<TBody>, IJsonResponse<TResponse>, IEndpointHandler
    {
        /// <summary>
        /// Processes json body and returns a new object
        /// </summary>
        /// <param name="body">Request body to be handled</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>Handled Response object</returns>
        Task<TResponse> Handle(TBody body, CancellationToken cancellationToken);
    }
}
