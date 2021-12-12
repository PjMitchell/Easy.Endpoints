using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class ContextEndpointParameterInfo : PredefinedEndpointParameterDeclaration
    {
        public static ContextEndpointParameterInfo Instance => new ContextEndpointParameterInfo();
        public override ValueTask<object?> Factory(HttpContext httpContext, EndpointOptions options) => ValueTask.FromResult<object?>(httpContext);
    }

}
