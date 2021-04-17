using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public interface IJsonResponseEndpointHandler<TResponse> : IJsonResponse<TResponse>, IEndpointHandler
    {
        Task<TResponse> Handle();
    }
}
