using Easy.Endpoints.TestService.Endpoints;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class CustomParserParameterModelBindingTests
    {
        private readonly TestServer server;
        private readonly Coordinates one = new (1.2, 2.4);
        private readonly Coordinates two = new(3.2, 2.2);
        private readonly Coordinates three = new(1.9, 5.4);



        public CustomParserParameterModelBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<CoordinatesEndpoint>(), new CoordinatesParser());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {

            var url = $"{three}/Test?single={one}&multiple={two}";
            var result = await server.CreateRequest(url).GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            var multiple1 = Assert.Single(observed.Multiple);
            Assert.Equal(two, multiple1);
            Assert.Equal(one, observed.Single);
            Assert.Equal(three, observed.Route);

        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?single={one}&single={one}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?single=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Get("{route}/Test")]
        public class CoordinatesEndpoint : IEndpoint
        {
            public UrlModel Handle(Coordinates route, Coordinates[] multiple, Coordinates single)
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
            public Coordinates Single { get; set; } = new();
            public Coordinates[] Multiple { get; set; } = Array.Empty<Coordinates>();

            public Coordinates Route { get; set; } = new();
        }
    }
}
