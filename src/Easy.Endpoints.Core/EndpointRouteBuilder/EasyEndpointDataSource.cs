using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    internal class EasyEndpointDataSource : EndpointDataSource
    {
        private readonly List<EasyEndpointConventionBuilder> endpointBuilders;
        public EasyEndpointDataSource()
        {
            endpointBuilders = new List<EasyEndpointConventionBuilder>();
        }

        public IEndpointConventionBuilder AddEndpointBuilder(EndpointBuilder builder)
        {
            var requestEndpointBuilder = new EasyEndpointConventionBuilder(builder);
            endpointBuilders.Add(requestEndpointBuilder);
            return requestEndpointBuilder;
        }

        public override IReadOnlyList<Endpoint> Endpoints => GenerateEnpoints();

        public override IChangeToken GetChangeToken()
        {
            return NullChangeToken.Singleton;
        }

        private IReadOnlyList<Endpoint> GenerateEnpoints() => endpointBuilders.Select(s => s.Build()).ToArray();
    }
}
