using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that processes a json body and returns an endpointResult
    /// </summary>
    /// <typeparam name="TBody">Type of Json Body</typeparam>
    public interface IJsonBodyEndpointResultHandler<TBody> : IJsonBody<TBody>, IEndpointHandler
    {
        /// <summary>
        /// Processes json body and returns a new endpoint result
        /// </summary>
        /// <param name="body">Request body to be handled</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>Endpoint result</returns>
        Task<IEndpointResult> HandleAsync(TBody body, CancellationToken cancellationToken);
    }
}
