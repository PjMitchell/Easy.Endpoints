using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class SwaggerGenTest
    {
        private readonly Microsoft.AspNetCore.TestHost.TestServer server;
        public SwaggerGenTest()
        {
            server = TestEndpointServerFactory.CreateEndpointServer();
        }

        [Fact]
        public async Task SwaggerGenMatchesExpectation()
        {
            var result = await server.CreateClient().GetAsync("/swagger/v1/swagger.json");
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            var observed = await result.GetJsonBody<TestOpenApiModel>();
            var expected = JsonSerializer.Deserialize<TestOpenApiModel>(File.ReadAllText("./ExpectedFiles/swagger.json"), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            if(expected is null)
            {
                Assert.NotNull(expected);
                return;
            }
            Assert.Equal(expected.Paths.Keys.OrderBy(o => o), observed.Paths.Keys.OrderBy(o => o));
        }

        [Fact]
        public async Task SwaggerGen_ProducedTypes()
        {
            var result = await server.CreateClient().GetAsync("/swagger/v1/swagger.json");
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            var observed = await result.GetJsonBody<TestOpenApiModel>();
            var peopleById = observed.Paths["/People/{id}"]["get"];
            Assert.Equal(new[] { "200", "404" }, peopleById.Responses.Keys);
        }

        public class TestOpenApiModel
        {
            public Dictionary<string, Dictionary<string, TestOperation>> Paths { get; set; } = new Dictionary<string, Dictionary<string, TestOperation>>();
        }

        public class TestOperation
        {
            public string[] Tags { get; set; } = Array.Empty<string>();
            public OperationParameter[] Parameters { get; set; } = Array.Empty<OperationParameter>();
            public Dictionary<string, OperationResponse> Responses { get; set; } = new Dictionary<string, OperationResponse>();
        }

        public class OperationParameter
        {
            public string Name { get; set; } = string.Empty;
            public string In { get; set; } = string.Empty;
            public bool Required { get; set; }
        }

        public class OperationResponse
        {
            public string Description { get; set; } = string.Empty;
        }
    }
}
