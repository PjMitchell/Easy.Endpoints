using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class SwaggerGenTest
    {
        private readonly Microsoft.AspNetCore.TestHost.TestServer server;
        public SwaggerGenTest()
        {
            server = CreateEndpointServer();
        }

        [Fact]
        public async Task SwaggerGenMatchesExpectation()
        {
            var result = await server.CreateClient().GetAsync("/swagger/v1/swagger.json");            
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        private static Microsoft.AspNetCore.TestHost.TestServer CreateEndpointServer()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Easy.Endpoints.TestServer.Endpoints.Startup>();
            
            return new Microsoft.AspNetCore.TestHost.TestServer(builder);
        }
    }

    
}
