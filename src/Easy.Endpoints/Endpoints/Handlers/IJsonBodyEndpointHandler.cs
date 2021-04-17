using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public interface IJsonBodyEndpointHandler<TBody> : IJsonBody<TBody>, INoContentResponse, IEndpointHandler
    {
        Task Handle(TBody body);
    }
}
