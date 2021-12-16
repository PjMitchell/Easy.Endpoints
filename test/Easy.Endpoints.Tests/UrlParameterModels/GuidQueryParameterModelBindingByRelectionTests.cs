using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public class GuidQueryParameterModelBindingByRelectionTests
    {
        private readonly Guid one = new Guid("231307f7-356e-4bef-9385-f5f41bb2c323");
        private readonly Guid two = new Guid("c0698fdf-69da-4326-8788-d489e6f5ba6f");
        private readonly Guid three = new Guid("10698fdf-81da-4326-8788-d489e6f5ba6f");

        private readonly TestServer server;

        public GuidQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.WithEndpoint<GuidEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest($"{three}/Test?single={one}&nullable={two}&multiple={one}&multiple={two}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(two, observed.Nullable);
            Assert.Equal(one, observed.Single);
            Assert.Equal(new[] { one, two }, observed.Multiple);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{three}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest($"{three}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Empty(observed.Multiple);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{three}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(Guid.Empty, observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?nullable=12&nullable=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }
    

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?single=12&single=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?nullable=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?single=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForMultiple_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?multiple=one&multiple={two}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Get("{route:guid}/Test")]
        public class GuidEndpoint : IEndpoint
        {
            public UrlModel Handle(Guid route, Guid? nullable, Guid[] multiple, Guid single = default)
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
            public Guid[] Multiple { get; set; } = Array.Empty<Guid>();
            public Guid Single { get; set; }
            public Guid? Nullable { get; set; }
            public Guid Route { get; set; }

        }
    }
}
