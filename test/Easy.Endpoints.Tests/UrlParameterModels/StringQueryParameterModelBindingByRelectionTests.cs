using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public class StringQueryParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;

        public StringQueryParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<UrlModelEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest("RouteValue/Test?single=one&multiple=a&multiple=b").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal("one", observed.Result.Single);
            Assert.Equal("RouteValue", observed.Result.Route);
            Assert.Equal(new[] { "a", "b" }, observed.Result.Multiple);

        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest("RouteValue/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Empty(observed.Result.Multiple);
        }

        [Fact]
        public async Task MissingSingle_EmptyString()
        {
            var result = await server.CreateRequest("RouteValue/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal("", observed.Result.Single);
        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest("RouteValue/Test?single=12&single=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "single"), error.Error);
        }

        [Get("{route}/Test")]
        public class UrlModelEndpoint : TestUrlParameterEndpoint<UrlModel> { }

        public class UrlModel : UrlParameterModel
        {
            private static readonly Action<UrlModel, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<UrlModel>();
            public string[] Multiple { get; set; } = Array.Empty<string>();
            public string Single { get; set; } = string.Empty;
            [RouteParameter]
            public string Route { get; set; } = string.Empty;


            public override void BindUrlParameters(HttpRequest request) => binding(this, request);
        }
    }
}
