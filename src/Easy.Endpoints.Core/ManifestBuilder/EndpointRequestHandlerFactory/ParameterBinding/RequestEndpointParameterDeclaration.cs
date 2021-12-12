using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class RequestEndpointParameterDeclaration : PredefinedEndpointParameterDeclaration
    {
        public static RequestEndpointParameterDeclaration Instance => new ();
        public override ValueTask<object?> Factory(HttpContext httpContext, EndpointOptions options) => ValueTask.FromResult<object?>(httpContext.Request);
    }

}
