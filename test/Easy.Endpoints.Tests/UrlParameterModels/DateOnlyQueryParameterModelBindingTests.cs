using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class DateOnlyQueryParameterModelBindingTests
    {
        private readonly TestServer server;
        private readonly DateOnly one = new DateOnly(2019, 10, 1);
        private readonly DateOnly two = new DateOnly(2018, 11, 4);
        private readonly DateOnly three = new DateOnly(2019, 11, 4);

        public DateOnlyQueryParameterModelBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.WithEndpoint<DateTimeEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {


            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test?single={one:yyyy-MM-dd}&nullable={two:yyyy-MM-dd}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(two, observed.Nullable);
            Assert.Equal(one, observed.Single);
            Assert.Equal(three, observed.Route);


        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(default(DateOnly), observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test?nullable={three:yyyy-MM-dd}&nullable={three:yyyy-MM-dd}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test?single={three:yyyy-MM-dd}&single={three:yyyy-MM-dd}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test?nullable=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test?single=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Get("{route}/Test")]
        public class DateTimeEndpoint : IEndpoint
        {
            public UrlModel Handle(DateOnly route, DateOnly? nullable, DateOnly single = default)
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
            public DateOnly Single { get; set; }
            public DateOnly? Nullable { get; set; }
            public DateOnly Route { get; set; }

        }
    }
}
