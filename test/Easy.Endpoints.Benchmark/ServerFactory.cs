using Easy.Endpoints.Benchmark.Endpoint;
using Easy.Endpoints.TestService.Endpoints.Books;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Endpoints.Benchmark
{
    public static class ServerFactory
    {
        public static TestServer CreateMvcServer()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddMvc();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapControllers());
                });

            return new TestServer(builder);
        }

        public static TestServer CreateEndpointServer()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddEasyEndpoints(b => b.AddForEndpointHandler<TestGetEndpoint>()
                    .AddForEndpointHandler<Test2GetEndpoint>()
                    .AddForEndpointHandler<GetBookEndpointHandler>()
                    .AddForEndpointHandler<PostBookEndpointHandler>()
                    );
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapEasyEndpoints());
                });

            return new TestServer(builder);
        }
    }
}
