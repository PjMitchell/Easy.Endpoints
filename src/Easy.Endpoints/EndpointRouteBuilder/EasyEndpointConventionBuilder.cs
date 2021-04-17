using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    public class EasyEndpointConventionBuilder : IEndpointConventionBuilder
    {
        private readonly EndpointBuilder endpointBuiilder;

        private readonly List<Action<EndpointBuilder>> conventions;

        public EasyEndpointConventionBuilder(EndpointBuilder endpointBuilder)
        {
            this.endpointBuiilder = endpointBuilder;
            conventions = new List<Action<EndpointBuilder>>();
        }

        public void Add(Action<EndpointBuilder> convention)
        {
            conventions.Add(convention);
        }

        public Endpoint Build()
        {
            foreach (var convention in conventions)
            {
                convention(endpointBuiilder);
            }

            return endpointBuiilder.Build();
        }
    }
}
