using BenchmarkDotNet.Attributes;
using Easy.Endpoints.Benchmark.Endpoint;
using Easy.Endpoints.TestService.Endpoints.People;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark
{

    [RankColumn, MemoryDiagnoser]
    public class UrlParameterBenchmarks
    {
        private HttpClient endPointServer;
        private HttpClient codeGenEndPointServer;
        private HttpClient mvcServer;

        [GlobalSetup]
        public void Setup()
        {
            endPointServer = ServerFactory.CreateEndpointServer(o => o.AddForEndpointHandler<GetPeopleEndpointHandler>()).CreateClient();
            codeGenEndPointServer = ServerFactory.CreateEndpointServer(o => o.AddForEndpointHandler<GetCodeGeneratedPeopleEndpointHandler>()).CreateClient();
            mvcServer = ServerFactory.CreateMvcServer().CreateClient();
        }

        [Benchmark]
        public async Task<string> GetEndPoint()
        {
            return await SendRequest(endPointServer, "/people?surname=Walpole").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> GetCodeGeneratedEndPoint()
        {
            return await SendRequest(codeGenEndPointServer, "/people?surname=Walpole").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> GetMvc()
        {
            return await SendRequest(mvcServer, "/people?surname=Walpole").ConfigureAwait(false);
        }

        private static async Task<string> SendRequest(HttpClient httpClient, string route = "/test1")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            var output = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return output;
        }

    }
}
