using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class UrlParameterModelBindingByRelectionTests
    {
        private readonly TestServer server;

        public UrlParameterModelBindingByRelectionTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<SimpleUrlModelFromQueryEndpoint>());
        }

        [Fact]
        public async Task CanMapSimpleModelThatPurelyContainsQueryParameters()
        {
            var result = await server.CreateRequest("Test/Simple?surname=par&forename=bob&age=42&size=10&size=20").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<SimpleUrlModelFromQuery>>();
            Assert.Equal(new[] { "bob" }, observed.Result.Name);
            Assert.Equal("par", observed.Result.Surname);
            Assert.Equal("Test", observed.Result.Route);
            Assert.Equal(42, observed.Result.Age);
            Assert.Equal(new[] { 10, 20 }, observed.Result.Size);

        }

        [Fact]
        public async Task CanMapSimpleModelThatPurelyContainsQueryParameters_ReturnsErrors()
        {
            var result = await server.CreateRequest("Test/Simple?surname=par&surname=bob&age=fortytwo&size=ten&size=20").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<SimpleUrlModelFromQuery>>();
            Assert.Equal(3, observed.Errors.Count);
            Assert.Single(observed.Errors, e => e.ParameterName == "surname");
            Assert.Single(observed.Errors, e => e.ParameterName == "age");
            Assert.Single(observed.Errors, e => e.ParameterName == "size");
        }

        [Get("{testRoute}/Simple")]
        public class SimpleUrlModelFromQueryEndpoint : TestUrlParameterEndpoint<SimpleUrlModelFromQuery> { }

        public class SimpleUrlModelFromQuery : UrlParameterModel
        {
            private static readonly Action<SimpleUrlModelFromQuery, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<SimpleUrlModelFromQuery>();

            [RouteParameter("testRoute")]
            public string Route { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            [QueryParameter("forename")]
            public string[] Name { get; set; } = Array.Empty<string>();
            public int[] Size { get; set; } = Array.Empty<int>();
            public int Age { get; set; }
            public override void BindUrlParameters(HttpRequest request) => binding(this, request);
        }
    }
}
