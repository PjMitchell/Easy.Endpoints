using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public class DateTimeQueryParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;
        private readonly DateTime one = new DateTime(2019, 10, 1, 9, 13, 42);
        private readonly DateTime two = new DateTime(2018, 11, 4, 8, 23, 12);
        private readonly DateTime three = new DateTime(2019, 11, 4, 8, 23, 12);



        public DateTimeQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<UrlModelEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {

          
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?single={one:yyyy-MM-ddTHH:mm:ss}&nullable={two:yyyy-MM-ddTHH:mm:ss}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(two, observed.Result.Nullable);
            Assert.Equal(one, observed.Result.Single);
            Assert.Equal(three, observed.Result.Route);


        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Null(observed.Result.Nullable);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(default(DateTime), observed.Result.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?nullable=12&nullable=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "nullable"), error.Error);

        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?single=12&single=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "single"), error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?nullable=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(DateTime)), error.Error);

        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:ss}/Test?single=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(DateTime)), error.Error);
        }

        [Get("{route:datetime}/Test")]
        public class UrlModelEndpoint : TestUrlParameterEndpoint<UrlModel> { }

        public class UrlModel : UrlParameterModel
        {
            private static readonly Action<UrlModel, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<UrlModel>();
            public DateTime Single { get; set; }
            public DateTime? Nullable { get; set; }
            [RouteParameter]
            public DateTime Route { get; set; }

            public override void BindUrlParameters(HttpRequest request) => binding(this, request);
        }
    }

    public class DateTimeOffSetQueryParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;
        private readonly DateTimeOffset one = new DateTimeOffset(new DateTime(2019, 10, 1, 9, 13, 42), TimeSpan.FromHours(-1));
        private readonly DateTimeOffset two = new DateTimeOffset(new DateTime(2018, 11, 4, 8, 23, 12), TimeSpan.FromHours(0));
        private readonly DateTimeOffset three = new DateTimeOffset(new DateTime(2017, 3, 5, 10, 15, 45), TimeSpan.FromHours(1));



        public DateTimeOffSetQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<UrlModelEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {

            var url = $"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?single={one:yyyy-MM-ddTHH:mm:sszzz}&nullable={two:yyyy-MM-ddTHH:mm:ssZ}";
            var result = await server.CreateRequest(url).GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(two, observed.Result.Nullable);
            Assert.Equal(one, observed.Result.Single);
            Assert.Equal(three, observed.Result.Route);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Null(observed.Result.Nullable);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(default(DateTimeOffset), observed.Result.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?nullable=12&nullable=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "nullable"), error.Error);

        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?single=12&single=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "single"), error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?nullable=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(DateTimeOffset)), error.Error);

        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three:yyyy-MM-ddTHH:mm:sszzz}/Test?single=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(DateTimeOffset)), error.Error);
        }

        [Get("{route:datetime}/Test")]
        public class UrlModelEndpoint : TestUrlParameterEndpoint<UrlModel> { }

        public class UrlModel : UrlParameterModel
        {
            private static readonly Action<UrlModel, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<UrlModel>();
            public DateTimeOffset Single { get; set; }
            public DateTimeOffset? Nullable { get; set; }
            [RouteParameter]
            public DateTimeOffset Route { get; set; }

            public override void BindUrlParameters(HttpRequest request) => binding(this, request);
        }
    }
}
