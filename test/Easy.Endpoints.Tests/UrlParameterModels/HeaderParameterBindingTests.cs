using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class HeaderParameterBindingTests
    {
        private const string multipleHeaderName = "x-TestM";
        public const string singleHeaderName = "x-TestS";
        protected readonly TestServer server;

        public HeaderParameterBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.WithEndpoint<IntHeaderEndpoint>().WithEndpoint<StringHeaderEndpoint>());
        }

        [Fact]
        public async Task Int_CanMapQueryParameters()
        {
            var result = await GetIntAsync(123, 1, 2);
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<EndpointModel<int>>();
            Assert.Equal(123, observed.Single);
            Assert.Equal(new[] { 1, 2 }, observed.Multiple);
        }

        [Fact]
        public async Task String_CanMapQueryParameters()
        {
            var result = await GetStringEndpointAsync("First", "Aone", "Btwo");
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<EndpointModel<string>>();
            Assert.Equal("First", observed.Single);
            Assert.Equal(new[] { "Aone", "Btwo" }, observed.Multiple);
        }



        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await GetStringEndpointAsync(null);
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<EndpointModel<string>>();
            Assert.Empty(observed.Multiple);
        }

        [Fact]
        public async Task Int_MissingSingle_Default()
        {
            var result = await GetIntAsync(null, "2");
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<EndpointModel<int>>();
            Assert.Equal(23, observed.Single);
        }

        [Fact]
        public async Task String_MissingSingle_Default()
        {
            var result = await GetStringEndpointAsync(null, "2");
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<EndpointModel<string>>();
            Assert.Equal("Nope", observed.Single);
        }


        [Fact]
        public async Task MultipleValues_ForSingle_Returns400()
        {
            var result = await server.CreateRequest("/Test").AddHeader(singleHeaderName, "1").AddHeader(singleHeaderName, "2").GetAsync();

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

        }

        [Fact]
        public async Task FailedToParses_ForSingle_Returns400()
        {
            var result = await GetIntAsync("nope", "2");
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Int_FailedToParses_ForMultiple_Returns400()
        {
            var result = await GetIntAsync("23", "2", "nope");
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        private Task<HttpResponseMessage> GetIntAsync(int? header1, params int[] multipleHeaders) => GetIntAsync(header1?.ToString(), multipleHeaders.Select(s=> s.ToString()).ToArray());

        private Task<HttpResponseMessage> GetIntAsync(string? header1, params string[] multipleHeaders) => GetAsync("/TestInt", header1, multipleHeaders);

        private Task<HttpResponseMessage> GetStringEndpointAsync(string? header1, params string[] multipleHeaders) => GetAsync("/Test", header1, multipleHeaders);


        private Task<HttpResponseMessage> GetAsync(string path, string? header1, params string[] multipleHeaders)
        {
            var requestBuilder = server.CreateRequest(path);
            if (!string.IsNullOrEmpty(header1))
                requestBuilder = requestBuilder.AddHeader(singleHeaderName, header1);
            foreach (var header in multipleHeaders)
                requestBuilder = requestBuilder.AddHeader(multipleHeaderName, header);
            return requestBuilder.GetAsync();
        }

        [Get("/TestInt")]
        public class IntHeaderEndpoint : IEndpoint
        {
            public EndpointModel<int> Handle([FromHeader(multipleHeaderName)]int[] multiple, [FromHeader(singleHeaderName)] int single = 23)
            {
                return new EndpointModel<int>
                {
                    Single = single,
                    Multiple = multiple
                };
            }
        }

        [Get("/Test")]
        public class StringHeaderEndpoint : IEndpoint
        {
            

            public EndpointModel<string> Handle([FromHeader(multipleHeaderName)] string[] multiple, [FromHeader(singleHeaderName)] string single = "Nope")
            {
                return new EndpointModel<string>
                {
                    Single = single,
                    Multiple = multiple
                };
            }
        }

        public class EndpointModel<T>
        {
            public T? Single { get; set; }
            public T[] Multiple { get; set; } = Array.Empty<T>();
        }
    }
}
