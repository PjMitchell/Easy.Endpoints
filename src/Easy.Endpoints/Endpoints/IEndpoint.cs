using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public interface IEndpoint
    {
        Task HandleRequest(HttpContext httpContext);
    }
}
