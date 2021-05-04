using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = await server.CreateRequest("Simple?surname=par&name=bob&age=42&size=10&size=20").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<SimpleUrlModelFromQuery>();
            Assert.Equal(new[] { "bob" }, observed.Name);
            Assert.Equal("par", observed.Surname);
            Assert.Equal(42, observed.Age);
            Assert.Equal(new[] { 10, 20 }, observed.Size);

        }

        [Fact]
        public async Task CanMapSimpleModelThatPurelyContainsQueryParameters_ReturnsErrors()
        {
            var result = await server.CreateRequest("Simple?surname=par&surname=bob&age=fortytwo&size=ten&size=20").GetAsync();
            Assert.False(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<List<UrlParameterModelError>>();
            Assert.Equal(3, observed.Count);
            Assert.Single(observed, e => e.ParameterName == "surname");
            Assert.Single(observed, e => e.ParameterName == "age");
            Assert.Single(observed, e => e.ParameterName == "size");


        }

        [Get("Simple")]
        public class SimpleUrlModelFromQueryEndpoint : TestEndpoint<SimpleUrlModelFromQuery> { }

        public abstract class TestEndpoint<T> : IEndpoint where T: UrlParameterModel, new()
        {
            public async Task HandleRequest(EndpointContext endpointContext)
            {
                var model = new T();
                model.BindUrlParameters(endpointContext.Request);
                if(model.IsModelValid())
                {
                    await endpointContext.Response.WriteAsJsonAsync(model);
                    return;
                }


                endpointContext.Response.StatusCode = 400;
                await endpointContext.Response.WriteAsJsonAsync(model.Errors.ToList());
            }
        }
    }

    public class SimpleUrlModelFromQuery : UrlParameterModel
    {
        private static readonly Action<SimpleUrlModelFromQuery, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<SimpleUrlModelFromQuery>();

        [FromQuery]
        public string Surname { get; set; } = string.Empty;
        public string[] Name { get; set; } = Array.Empty<string>();
        public int[] Size { get; set; } = Array.Empty<int>();
        public int Age { get; set; }


        public override void BindUrlParameters(HttpRequest request) => binding(this, request);
    }
}
