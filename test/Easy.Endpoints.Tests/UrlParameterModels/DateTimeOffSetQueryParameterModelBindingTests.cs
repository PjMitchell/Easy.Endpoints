using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class DateTimeOffSetQueryParameterModelBindingTests
    {
        private readonly TestServer server;
        private readonly DateTimeOffset one = new DateTimeOffset(new DateTime(2019, 10, 1, 9, 13, 42), TimeSpan.FromHours(-1));
        private readonly DateTimeOffset two = new DateTimeOffset(new DateTime(2018, 11, 4, 8, 23, 12), TimeSpan.FromHours(0));
        private readonly DateTimeOffset three = new DateTimeOffset(new DateTime(2017, 3, 5, 10, 15, 45), TimeSpan.FromHours(1));



        public DateTimeOffSetQueryParameterModelBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<DateTimeOffsetEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {

            var url = $"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?single={one:yyyy-MM-ddTHH:mm:sszzz}&nullable={two:yyyy-MM-ddTHH:mm:ssZ}";
            var result = await server.CreateRequest(url).GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(two, observed.Nullable);
            Assert.Equal(one, observed.Single);
            Assert.Equal(three, observed.Route);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(default(DateTimeOffset), observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?nullable={three:yyyy-MM-ddTHH:mm:sszzz}&nullable={three:yyyy-MM-ddTHH:mm:sszzz}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?single={three:yyyy-MM-ddTHH:mm:sszzz}&single={three:yyyy-MM-ddTHH:mm:sszzz}").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?nullable=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?single=one").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Get("{route:datetime}/Test")]
        public class DateTimeOffsetEndpoint : IEndpoint
        {
            public UrlModel Handle(DateTimeOffset route, DateTimeOffset? nullable, DateTimeOffset single = default)
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
            public DateTimeOffset Single { get; set; }
            public DateTimeOffset? Nullable { get; set; }
            public DateTimeOffset Route { get; set; }
        }
    }
}
