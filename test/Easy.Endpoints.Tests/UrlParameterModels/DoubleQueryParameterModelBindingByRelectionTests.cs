using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public class DoubleQueryParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;

        public DoubleQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<DoubleEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest("1.4/Test?single=1.2&nullable=2.3&multiple=1.1&multiple=2.2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(2.3, observed.Nullable);
            Assert.Equal(1.2, observed.Single);
            Assert.Equal(1.4, observed.Route);
            Assert.Equal(new double[] { 1.1, 2.2 }, observed.Multiple);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest("1.2/Test?single=12&multiple=1&multiple=2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest("1.2/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Empty(observed.Multiple);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest("1.2/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(0, observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?nullable=12&nullable=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }
    

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?single=12&single=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?nullable=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?single=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForMultiple_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?multiple=one&multiple=2").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Get("{route:double}/Test")]
        public class DoubleEndpoint : IEndpoint
        {
            public UrlModel Handle(double route, double? nullable, double[] multiple, double single = 0)
            {
                return new UrlModel
                {
                    Route = route,
                    Single = single,
                    Nullable = nullable,
                    Multiple = multiple
                };
            }
        }
        public class UrlModel
        {
            public double[] Multiple { get; set; } = Array.Empty<double>();
            public double Single { get; set; }
            public double? Nullable { get; set; }
            public double Route { get; set; }
        }
    }
}
