using Easy.Endpoints.TestService.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Easy.Endpoints.Tests
{
    public static class TestEndpointServerFactory
    {
        public static TestServer CreateEndpointServer()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();

            return new TestServer(builder);
        }

        public static TestServer CreateEndpointServer(Action<EndpointManifestBuilder> manifestBuilderActions)
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddRequestEndpoints(manifestBuilderActions);
                })
                .Configure(app =>
                {
                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.AddEasyEndpoints();
                    });
                });

            return new Microsoft.AspNetCore.TestHost.TestServer(builder);
        }
    }
}
