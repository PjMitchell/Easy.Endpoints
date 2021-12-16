using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class ComplexQueryParameterModelBindingTests
    {
        private readonly TestServer server;
        
        public ComplexQueryParameterModelBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.WithEndpoint<TestEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest("/Test?one=2&two=2.4&multiple=2&multiple=5").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<GetTestModel>();
            Assert.Equal(2, observed.One);
            Assert.Equal(2.4m, observed.Two);
            Assert.Equal(new[] { 2, 5 }, observed.Multiple);
        }

        [Get("/Test")]
        public class TestEndpoint : IEndpoint
        {
            public GetTestModel Handle([FromQuery] GetTestModel model)
            {
                return model;
            }
        }

        public class GetTestModel
        {
            public int One { get; init; }
            public decimal? Two { get; init; }
            public int[] Multiple { get; init; } = Array.Empty<int>();

        }
    }
}
