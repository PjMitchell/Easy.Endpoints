using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public interface IJsonEndpointHandler<TBody, TResponse> : IJsonBody<TBody>, IJsonResponse<TResponse>, IEndpointHandler
    {
        Task<TResponse> Handle(TBody body);
    }
}
