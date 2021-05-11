using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that returns an endpoint result
    /// </summary>
    public interface IEndpointResultHandler : IEndpointHandler
    {
        /// <summary>
        /// Processes endpoint returning an endpoint result;
        /// </summary>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>Endpoint result for request</returns>
        Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken);
    }
}
