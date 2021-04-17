using System.Collections.Generic;

namespace Easy.Endpoints
{
    public class EndpointManifestBuilder
    {
        private readonly List<EndpointInfo> source;

        public EndpointManifestBuilder()
        {
            source = new List<EndpointInfo>();
        }

        public void AddEndpoint(EndpointInfo endpoint)
        {
            source.Add(endpoint);
        }

        public IEndpointManifest Build()
        {
            return new EndpointManifest(source);
        }
    }
}
