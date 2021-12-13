using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class UserEndpointParameterDeclaration : PredefinedEndpointParameterDeclaration
    {
        public static UserEndpointParameterDeclaration Instance => new ();
        public override ValueTask<object?> Factory(HttpContext httpContext, EndpointOptions options) => ValueTask.FromResult<object?>(httpContext.User);
    }

}
