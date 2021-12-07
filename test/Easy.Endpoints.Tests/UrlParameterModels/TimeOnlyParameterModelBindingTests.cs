using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class TimeOnlyParameterModelBindingTests
    {
        private readonly TestServer server;
        private readonly TimeOnly one = new TimeOnly(9, 13, 42);
        private readonly TimeOnly two = new TimeOnly(8, 23, 12);
        private readonly TimeOnly three = new TimeOnly(8, 23, 12);



        public TimeOnlyParameterModelBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<DateTimeEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {


            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test?single={one:HH:mm:ss}&nullable={two:HH:mm:ss}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(two, observed.Nullable);
            Assert.Equal(one, observed.Single);
            Assert.Equal(three, observed.Route);


        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(default(TimeOnly), observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test?nullable={two:HH:mm:ss}&nullable={two:HH:mm:ss}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test?single={two:HH:mm:ss}&single={two:HH:mm:ss}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test?nullable=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test?single=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Get("{route}/Test")]
        public class DateTimeEndpoint : IEndpoint
        {
            public UrlModel Handle(TimeOnly route, TimeOnly? nullable, TimeOnly single = default)
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
            public TimeOnly Single { get; set; }
            public TimeOnly? Nullable { get; set; }
            public TimeOnly Route { get; set; }

        }
    }
}
