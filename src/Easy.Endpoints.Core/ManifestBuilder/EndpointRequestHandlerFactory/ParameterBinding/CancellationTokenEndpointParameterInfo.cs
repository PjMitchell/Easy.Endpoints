using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class CancellationTokenEndpointParameterInfo : PredefinedEndpointParameterDeclaration
    {
        public static CancellationTokenEndpointParameterInfo Instance => new CancellationTokenEndpointParameterInfo();
        public override ValueTask<object?> Factory(HttpContext httpContext, EndpointOptions options) => ValueTask.FromResult<object?>(httpContext.RequestAborted);
    }

}
