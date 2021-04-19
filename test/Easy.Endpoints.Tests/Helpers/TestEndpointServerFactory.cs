using Microsoft.AspNetCore.Hosting;

namespace Easy.Endpoints.Tests
{
    public static class TestEndpointServerFactory
    {
        public static Microsoft.AspNetCore.TestHost.TestServer CreateEndpointServer()
        {
            var builder = new WebHostBuilder()
                .UseStartup<TestServer.Endpoints.Startup>();

            return new Microsoft.AspNetCore.TestHost.TestServer(builder);
        }
    }
}
