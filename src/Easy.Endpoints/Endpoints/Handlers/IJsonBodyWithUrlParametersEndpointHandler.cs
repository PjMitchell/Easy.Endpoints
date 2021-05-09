using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{

    /// <summary>
    /// A handler that processes a json body with Url Parameter Model and will return a 201 response
    /// </summary>
    /// <typeparam name="TBody">Type of Json Body</typeparam>
    /// <typeparam name="TUrlParameterModel">Model representing Url Paramter</typeparam>
    public interface IJsonBodyWithUrlParametersEndpointHandler<TBody, TUrlParameterModel> : IJsonBody<TBody>, IUrlParameterModel<TUrlParameterModel>, INoContentResponse, IEndpointHandler
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        /// <summary>
        /// Processes body and url parameters
        /// </summary>
        /// <param name="body">Request body to be handled</param>
        /// <param name="urlParameters">Model containing url Parameter values</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>A Task</returns>
        Task HandleAsync(TBody body, TUrlParameterModel urlParameters, CancellationToken cancellationToken);
    }
}
