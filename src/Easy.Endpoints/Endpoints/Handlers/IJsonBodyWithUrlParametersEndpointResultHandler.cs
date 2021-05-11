using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that processes a JSON body with url parameters and returns an endpoint result
    /// </summary>
    /// <typeparam name="TBody">Type of JSON Body</typeparam>
    /// <typeparam name="TUrlParameterModel">Model representing Url Parameter</typeparam>
    public interface IJsonBodyWithUrlParametersEndpointResultHandler<TBody, TUrlParameterModel> : IJsonBody<TBody>, IUrlParameterModel<TUrlParameterModel>, IEndpointHandler
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        /// <summary>
        ///  Processes JSON body with url parameters and returns a new object
        /// </summary>
        /// <param name="body">Request body to be handled</param>
        /// <param name="urlParameters">Model containing url Parameter values</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>Endpoint result</returns>
        Task<IEndpointResult> HandleAsync(TBody body, TUrlParameterModel urlParameters, CancellationToken cancellationToken);
    }
}
