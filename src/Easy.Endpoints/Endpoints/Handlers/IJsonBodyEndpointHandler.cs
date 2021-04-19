using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that processes a json body and will return a 201 response
    /// </summary>
    /// <typeparam name="TBody">Type of Json Body</typeparam>
    public interface IJsonBodyEndpointHandler<TBody> : IJsonBody<TBody>, INoContentResponse, IEndpointHandler
    {
        /// <summary>
        /// Processes body
        /// </summary>
        /// <param name="body">Request body to be handled</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>A Task</returns>
        Task Handle(TBody body, CancellationToken cancellationToken);
    }
}
