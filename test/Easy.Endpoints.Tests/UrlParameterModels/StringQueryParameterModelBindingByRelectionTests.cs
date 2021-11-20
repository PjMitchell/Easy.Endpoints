using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public class StringQueryParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;

        public StringQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<StringEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest("RouteValue/Test?single=one&multiple=a&multiple=b").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal("one", observed.Single);
            Assert.Equal("RouteValue", observed.Route);
            Assert.Equal(new[] { "a", "b" }, observed.Multiple);

        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest("RouteValue/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Empty(observed.Multiple);
        }

        [Fact]
        public async Task MissingSingle_EmptyString()
        {
            var result = await server.CreateRequest("RouteValue/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal("", observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("RouteValue/Test?single=12&single=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Get("{route}/Test")]
        public class StringEndpoint : IEndpoint
        {
            public UrlModel Handle(string route, string[] multiple, string single = "")
            {
                return new UrlModel
                {
                    Route = route,
                    Single = single,
                    Multiple = multiple
                };
            }
        }

        public class UrlModel
        {
            public string[] Multiple { get; set; } = Array.Empty<string>();
            public string Single { get; set; } = string.Empty;
            public string Route { get; set; } = string.Empty;
        }
    }
}
