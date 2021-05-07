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
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<UrlModelEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest("1.4/Test?single=1.2&nullable=2.3&multiple=1.1&multiple=2.2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(2.3, observed.Result.Nullable);
            Assert.Equal(1.2, observed.Result.Single);
            Assert.Equal(1.4, observed.Result.Route);
            Assert.Equal(new double[] { 1.1, 2.2 }, observed.Result.Multiple);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest("1.2/Test?single=12&multiple=1&multiple=2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Null(observed.Result.Nullable);
        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest("1.2/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Empty(observed.Result.Multiple);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest("1.2/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(0, observed.Result.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?nullable=12&nullable=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "nullable"), error.Error);

        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?single=12&single=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "single"), error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?nullable=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(double)), error.Error);

        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?single=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(double)), error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForMultiple_ReturnsError()
        {
            var result = await server.CreateRequest("1.2/Test?multiple=one&multiple=2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("multiple", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(double)), error.Error);
        }

        [Get("{route:double}/Test")]
        public class UrlModelEndpoint : TestUrlParameterEndpoint<UrlModel> { }

        public class UrlModel : UrlParameterModel
        {
            private static readonly Action<UrlModel, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<UrlModel>();
            public double[] Multiple { get; set; } = Array.Empty<double>();
            public double Single { get; set; }
            public double? Nullable { get; set; }
            [RouteParameter]
            public double Route { get; set; }
            public override void BindUrlParameters(HttpRequest request) => binding(this, request);
        }
    }
}
