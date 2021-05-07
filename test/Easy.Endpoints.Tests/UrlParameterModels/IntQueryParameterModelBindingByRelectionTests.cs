using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public class IntQueryParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;

        public IntQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<UrlModelEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest("41/Test?single=12&nullable=23&multiple=1&multiple=2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(23, observed.Result.Nullable);
            Assert.Equal(12, observed.Result.Single);
            Assert.Equal(41, observed.Result.Route);
            Assert.Equal(new[] { 1, 2 }, observed.Result.Multiple);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest("41/Test?single=12&multiple=1&multiple=2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Null(observed.Result.Nullable);
        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest("41/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Empty(observed.Result.Multiple);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest("41/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(0, observed.Result.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("41/Test?nullable=12&nullable=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "nullable"), error.Error);

        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("41/Test?single=12&single=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "single"), error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest("41/Test?nullable=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(int)), error.Error);

        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("41/Test?single=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(int)), error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForMultiple_ReturnsError()
        {
            var result = await server.CreateRequest("41/Test?multiple=one&multiple=2").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("multiple", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(int)), error.Error);
        }

        [Get("{route:int}/Test")]
        public class UrlModelEndpoint : TestUrlParameterEndpoint<UrlModel> { }

        public class UrlModel : UrlParameterModel
        {
            private static readonly Action<UrlModel, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<UrlModel>();
            public int[] Multiple { get; set; } = Array.Empty<int>();
            public int Single { get; set; }
            public int? Nullable { get; set; }
            [RouteParameter]
            public int Route { get; set; }

            public override void BindUrlParameters(HttpRequest request) => binding(this, request);
        }
    }
}
