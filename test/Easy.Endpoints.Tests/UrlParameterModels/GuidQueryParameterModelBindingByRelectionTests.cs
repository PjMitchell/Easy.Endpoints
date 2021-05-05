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
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<UrlModelEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest($"{three}/Test?single={one}&nullable={two}&multiple={one}&multiple={two}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(two, observed.Result.Nullable);
            Assert.Equal(one, observed.Result.Single);
            Assert.Equal(new[] { one, two }, observed.Result.Multiple);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{three}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Null(observed.Result.Nullable);
        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest($"{three}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Empty(observed.Result.Multiple);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{three}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            Assert.Empty(observed.Errors);
            Assert.Equal(Guid.Empty, observed.Result.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?nullable=12&nullable=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "nullable"), error.Error);

        }

        [Fact]
        public async Task MultipleValues_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?single=12&single=12").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.MultipleParametersFoundError, "single"), error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?nullable=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(Guid)), error.Error);

        }

        [Fact]
        public async Task FailedToParses_ForSingle_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?single=one").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(Guid)), error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForMultiple_ReturnsError()
        {
            var result = await server.CreateRequest($"{three}/Test?multiple=one&multiple={two}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<UrlModel>>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("multiple", error.ParameterName);
            Assert.Equal(string.Format(UrlParameterErrorMessages.CouldNotParseError, "one", typeof(Guid)), error.Error);
        }

        [Get("{route:guid}/Test")]
        public class UrlModelEndpoint : TestUrlParameterEndpoint<UrlModel> { }

        public class UrlModel : UrlParameterModel
        {
            private static readonly Action<UrlModel, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<UrlModel>();
            public Guid[] Multiple { get; set; } = Array.Empty<Guid>();
            public Guid Single { get; set; }
            public Guid? Nullable { get; set; }
            [RouteParameter]
            public Guid Route { get; set; }

            public override void BindUrlParameters(HttpRequest request) => binding(this, request);
        }
    }
}
