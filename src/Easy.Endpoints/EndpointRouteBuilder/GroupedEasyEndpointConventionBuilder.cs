using Microsoft.AspNetCore.Builder;
using System;

namespace Easy.Endpoints
{
    internal class GroupedEasyEndpointConventionBuilder : IEndpointConventionBuilder
    {
        private readonly IEndpointConventionBuilder[] endpointBuilders;

        public GroupedEasyEndpointConventionBuilder(params IEndpointConventionBuilder[] builders)
        {
            endpointBuilders = builders;
        }

        public void Add(Action<EndpointBuilder> convention)
        {
            for (int i = 0; i < endpointBuilders.Length; i++)
            {
                endpointBuilders[i].Add(convention);
            }
        }
    }
}
