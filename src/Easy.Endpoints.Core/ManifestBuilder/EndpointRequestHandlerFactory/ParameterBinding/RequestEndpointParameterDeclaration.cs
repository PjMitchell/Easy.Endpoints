using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class RequestEndpointParameterDeclaration : PredefinedEndpointParameterDeclaration
    {
        public static RequestEndpointParameterDeclaration Instance => new ();
        public override ValueTask<ParameterBindingResult> Factory(HttpContext httpContext, EndpointOptions options, IBindingErrorCollection bindingErrorCollection) => ValueTask.FromResult(new ParameterBindingResult(httpContext.Request));
    }

}
