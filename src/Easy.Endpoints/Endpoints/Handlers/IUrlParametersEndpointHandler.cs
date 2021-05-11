using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A handler that processes a Url Parameter Model and will return a 201 response
    /// </summary>
    /// <typeparam name="TUrlParameterModel">Model representing Url Paramter</typeparam>
    public interface IUrlParametersEndpointHandler<TUrlParameterModel> : IUrlParameterModel<TUrlParameterModel>, INoContentResponse, IEndpointHandler
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        /// <summary>
        /// Processes url parameters
        /// </summary>
        /// <param name="urlParameters">Model containing url Parameter values</param>
        /// <param name="cancellationToken">Request Aborted</param>
        /// <returns>A Task</returns>
        Task HandleAsync(TUrlParameterModel urlParameters, CancellationToken cancellationToken);
    }
}
