using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class UserEndpointParameterDeclaration : PredefinedEndpointParameterDeclaration
    {
        public static UserEndpointParameterDeclaration Instance => new ();
        public override ValueTask<ParameterBindingResult> Factory(HttpContext httpContext, EndpointOptions options, IBindingErrorCollection bindingErrorCollection) => ValueTask.FromResult(new ParameterBindingResult(httpContext.User));
    }

}
