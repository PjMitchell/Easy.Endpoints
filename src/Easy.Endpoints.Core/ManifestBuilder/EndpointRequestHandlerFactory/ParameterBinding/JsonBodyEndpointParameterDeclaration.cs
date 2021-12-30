using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonBodyEndpointParameterDeclaration : EndpointHandlerParameterDeclaration
    {
        private readonly EndpointParameterDescriptor descriptor;

        public JsonBodyEndpointParameterDeclaration(Type parameterType, string name)
        {
            descriptor = new EndpointParameterDescriptor(EndpointParameterSource.Body, parameterType, name);
        }

        public override IEnumerable<EndpointParameterDescriptor> GetParameterDescriptors()
        {
            yield return descriptor;
        }

        public override ValueTask<ParameterBindingResult> Factory(HttpContext httpContext, EndpointOptions options, IBindingErrorCollection bindingErrorCollection)
        {
            return HttpContextJsonHelper.ReadJsonBody(httpContext, descriptor.ParameterType, options.JsonSerializerOptions, bindingErrorCollection);
        }
    }

}
