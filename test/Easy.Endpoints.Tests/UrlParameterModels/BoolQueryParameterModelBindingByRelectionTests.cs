using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class BoolQueryParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;

        public BoolQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<BoolEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest("True/Test?single=true&nullable=false").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(false, observed.Nullable);
            Assert.True(observed.Single);
            Assert.True(observed.Route);
        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest("True/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest("True/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.False(observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("True/Test?nullable=true&nullable=false").GetAsync();
            Assert.False(result.IsSuccessStatusCode);        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("True/Test?single=true&single=false").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("True/Test?nullable=nope").GetAsync();
            Assert.False(result.IsSuccessStatusCode);        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("True/Test?single=yeah").GetAsync();
            Assert.False(result.IsSuccessStatusCode);        }

        [Get("{route:bool}/Test")]
        public class BoolEndpoint : IEndpoint 
        {
            public UrlModel Handle(bool route, bool? nullable, bool single = false)
            {
                return new UrlModel
                {
                    Route = route,
                    Single = single,
                    Nullable = nullable
                };
            }
        }

        public class UrlModel
        {
            public bool Route { get; set; }
            public bool Single { get; set; }
            public bool? Nullable { get; set; }

        }
    }
}
