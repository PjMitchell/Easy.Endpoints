using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that processes a Url Parameter Model and returns an endpoint result
    /// </summary>
    /// <typeparam name="TUrlParameterModel">Model representing Url Parameter</typeparam>
    public interface IUrlParametersEndpointResultHandler<TUrlParameterModel> : IUrlParameterModel<TUrlParameterModel>, IEndpointHandler
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        /// <summary>
        /// Processes url parameters and returns an endpoint result;
        /// </summary>
        /// <param name="urlParameters">Model containing url Parameter values</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>Endpoint result for request</returns>
        Task<IEndpointResult> HandleAsync(TUrlParameterModel urlParameters, CancellationToken cancellationToken);
    }
}
