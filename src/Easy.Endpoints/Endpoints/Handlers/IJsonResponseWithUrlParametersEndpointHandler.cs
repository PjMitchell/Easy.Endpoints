using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that takes url parameters and returns a new object
    /// </summary>
    /// <typeparam name="TUrlParameterModel">Model representing Url Paramter</typeparam>
    /// <typeparam name="TResponse">Type of Json Response</typeparam>
    public interface IJsonResponseWithUrlParametersEndpointHandler<TUrlParameterModel, TResponse> : IUrlParameterModel<TUrlParameterModel>, IJsonResponse<TResponse>, IEndpointHandler
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        /// <summary>
        /// Handles a response and returns a new object
        /// </summary>
        /// <param name="urlParameters">Model containing url Parameter values</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>Handled Response object</returns>
        Task<TResponse> Handle(TUrlParameterModel urlParameters, CancellationToken cancellationToken);
    }
}
