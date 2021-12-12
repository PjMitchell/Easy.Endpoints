using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class ResponseEndpointParameterDeclaration : PredefinedEndpointParameterDeclaration
    {
        public static ResponseEndpointParameterDeclaration Instance => new ();
        public override ValueTask<object?> Factory(HttpContext httpContext, EndpointOptions options) => ValueTask.FromResult<object?>(httpContext.Response);
    }

}
