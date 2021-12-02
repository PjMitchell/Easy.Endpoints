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
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<DateTimeEndpoint>());
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
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?nullable=12&nullable=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?single=12&single=12").GetAsync();
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
            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test?nullable=12&nullable=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:HH:mm:ss}/Test?single=12&single=12").GetAsync();
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

    public class DateOnlyQueryParameterModelBindingTests
    {
        private readonly TestServer server;
        private readonly DateOnly one = new DateOnly(2019, 10, 1);
        private readonly DateOnly two = new DateOnly(2018, 11, 4);
        private readonly DateOnly three = new DateOnly(2019, 11, 4);

        public DateOnlyQueryParameterModelBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<DateTimeEndpoint>());
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
            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test?nullable=12&nullable=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-dd}/Test?single=12&single=12").GetAsync();
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
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?nullable=12&nullable=12").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?single=12&single=12").GetAsync();
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
