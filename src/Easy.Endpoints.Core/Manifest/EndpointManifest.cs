using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    internal class EndpointManifest : IEndpointManifest
    {
        private readonly IEnumerable<EndpointInfo> source;

        public EndpointManifest(IEnumerable<EndpointInfo> source)
        {
            this.source = source.ToArray();
        }

        public IEnumerator<EndpointInfo> GetEnumerator() => source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
