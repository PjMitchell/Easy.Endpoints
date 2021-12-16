using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    internal class EndpointManifest : IEndpointManifest
    {
        private readonly IEnumerable<EndpointDeclaration> source;

        public EndpointManifest(IEnumerable<EndpointDeclaration> source)
        {
            this.source = source.ToArray();
        }

        public IEnumerator<EndpointDeclaration> GetEnumerator() => source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
