using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark
{
    [RankColumn, MemoryDiagnoser]
    public class SampleHttpJsonGet
    {
        private HttpClient endPointServer;
        private HttpClient mvcServer;

        [GlobalSetup]
        public void Setup()
        {
            endPointServer = CreateEndpointServer().CreateClient();
            mvcServer = CreateMvcServer().CreateClient();
        }

        [Benchmark]
        public async Task<string> EndPoint()
        {
            return await SendRequest(endPointServer).ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> EndPoint2()
        {
            return await SendRequest(endPointServer, "/test2").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> Mvc()
        {
            return await SendRequest(mvcServer).ConfigureAwait(false);
        }

        private static async Task<string> SendRequest(HttpClient httpClient, string route = "/test1")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            var output = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return output;
        }

        private TestServer CreateEndpointServer()
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

            return new TestServer(builder);
        }

        private TestServer CreateMvcServer()
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

            return new TestServer(builder);
        }
    }
}
