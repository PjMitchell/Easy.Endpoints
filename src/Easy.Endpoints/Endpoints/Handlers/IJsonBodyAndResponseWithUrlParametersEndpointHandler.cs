using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that processes a JSON body with url parameters and returns a new object
    /// </summary>
    /// <typeparam name="TBody">Type of Json Body</typeparam>
    /// <typeparam name="TUrlParameterModel">Model representing Url Paramter</typeparam>
    /// <typeparam name="TResponse">Type of Json Response</typeparam>
    public interface IJsonBodyAndResponseWithUrlParametersEndpointHandler<TBody, TUrlParameterModel, TResponse> : IJsonBody<TBody>, IUrlParameterModel<TUrlParameterModel>, IJsonResponse<TResponse>, IEndpointHandler
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        /// <summary>
        ///  Processes JSON body with url parameters and returns a new object
        /// </summary>
        /// <param name="body">Request body to be handled</param>
        /// <param name="urlParameters">Model containing url Parameter values</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>Handled Response object</returns>
        Task<TResponse> HandleAsync(TBody body, TUrlParameterModel urlParameters, CancellationToken cancellationToken);
    }
}
