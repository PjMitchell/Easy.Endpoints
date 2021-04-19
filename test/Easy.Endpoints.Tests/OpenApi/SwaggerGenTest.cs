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
        }
    }    
}
