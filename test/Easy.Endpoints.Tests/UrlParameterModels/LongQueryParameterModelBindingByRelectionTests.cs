using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public class LongQueryParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;

        public LongQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<LongEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest("42/Test?single=12&nullable=23&multiple=1&multiple=2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(23, observed.Nullable);
            Assert.Equal(12, observed.Single);
            Assert.Equal(42, observed.Route);
            Assert.Equal(new long[] { 1, 2 }, observed.Multiple);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest("42/Test?single=12&multiple=1&multiple=2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest("42/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Empty(observed.Multiple);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest("42/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(0, observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("42/Test?nullable=12&nullable=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }
    

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("42/Test?single=12&single=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("42/Test?nullable=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("42/Test?single=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForMultiple_ReturnsError()
        {
            var result = await server.CreateRequest("42/Test?multiple=one&multiple=2").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }
        

        [Get("{route:long}/Test")]
        public class LongEndpoint : IEndpoint
        {
            public UrlModel Handle(long route, long? nullable, long[] multiple, long single = default)
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
            public long[] Multiple { get; set; } = Array.Empty<long>();
            public long Single { get; set; }
            [RouteParameter]
            public long Route { get; set; }

            public long? Nullable { get; set; }

        }
    }
}
