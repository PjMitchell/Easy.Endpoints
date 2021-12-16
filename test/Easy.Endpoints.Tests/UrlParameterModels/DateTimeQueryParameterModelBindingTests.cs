using Microsoft.AspNetCore.TestHost;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public class DateTimeParameterModelBindingTests
    {
        private readonly TestServer server;
        private readonly DateTime one = new DateTime(2019, 10, 1, 9, 13, 42);
        private readonly DateTime two = new DateTime(2018, 11, 4, 8, 23, 12);
        private readonly DateTime three = new DateTime(2019, 11, 4, 8, 23, 12);



        public DateTimeParameterModelBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.WithEndpoint<DateTimeEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {

          
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?single={one:yyyy-MM-ddTHH:mm:ss}&nullable={two:yyyy-MM-ddTHH:mm:ss}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(two, observed.Nullable);
            Assert.Equal(one, observed.Single);
            Assert.Equal(three, observed.Route);


        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(default(DateTime), observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?nullable={three:yyyy-MM-ddTHH:mm:ss}&nullable={three:yyyy-MM-ddTHH:mm:ss}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?single={three:yyyy-MM-ddTHH:mm:ss}&single={three:yyyy-MM-ddTHH:mm:ss}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?nullable=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?single=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Get("{route:datetime}/Test")]
        public class DateTimeEndpoint : IEndpoint
        {
            public UrlModel Handle(DateTime route, DateTime? nullable, DateTime single = default)
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
            public DateTime Single { get; set; }
            public DateTime? Nullable { get; set; }
            public DateTime Route { get; set; }

        }
    }
}
