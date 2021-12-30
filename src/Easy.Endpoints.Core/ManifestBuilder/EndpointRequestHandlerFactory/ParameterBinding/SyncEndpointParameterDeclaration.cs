using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class SyncEndpointParameterDeclaration : EndpointHandlerParameterDeclaration
    {
        public EndpointParameterDescriptor[] Descriptors { get; init; }

        public SyncParameterFactory ParameterFactory { get; init; }

        public SyncEndpointParameterDeclaration(SyncParameterFactory parameterFactory, EndpointParameterDescriptor[] descriptors)
        {
            ParameterFactory = parameterFactory;
            Descriptors = descriptors;
        }

        public override IEnumerable<EndpointParameterDescriptor> GetParameterDescriptors() => Descriptors;

        public override ValueTask<ParameterBindingResult> Factory(HttpContext httpContext, EndpointOptions options, IBindingErrorCollection bindingErrorCollection) => ValueTask.FromResult(ParameterFactory(httpContext, options, bindingErrorCollection));

    }

    internal delegate ParameterBindingResult SyncParameterFactory(HttpContext httpContext, EndpointOptions options, IBindingErrorCollection errorCollection);

}
