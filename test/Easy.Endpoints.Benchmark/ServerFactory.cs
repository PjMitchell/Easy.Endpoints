using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Endpoints.Benchmark
{
    public static class ServerFactory
    {
        public static Microsoft.AspNetCore.TestHost.TestServer CreateMvcServer()
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

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });

            return new Microsoft.AspNetCore.TestHost.TestServer(builder);
        }

        public static Microsoft.AspNetCore.TestHost.TestServer CreateEndpointServer()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddRequestEndpoints();
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
