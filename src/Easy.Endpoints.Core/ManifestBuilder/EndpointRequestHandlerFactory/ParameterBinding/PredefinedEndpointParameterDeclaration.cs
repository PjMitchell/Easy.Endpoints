using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    internal abstract class PredefinedEndpointParameterDeclaration : EndpointHandlerParameterDeclaration
    {
        public override IEnumerable<EndpointParameterDescriptor> GetParameterDescriptors() => Enumerable.Empty<EndpointParameterDescriptor>();
    }

}
