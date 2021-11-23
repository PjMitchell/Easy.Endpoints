using BenchmarkDotNet.Attributes;
using System.Net.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark
{
    [RankColumn, MemoryDiagnoser]
    public class BookHttpJsonGet
    {
        private HttpClient endPointServer;
        private HttpClient mvcServer;
        private HttpClient minimalServer;


        [GlobalSetup]
        public void Setup()
        {
            endPointServer = ServerFactory.CreateEndpointServer().CreateClient();
            mvcServer = ServerFactory.CreateMvcServer().CreateClient();
            minimalServer = ServerFactory.CreateMinimalApiServer().CreateClient();
        }

        [Benchmark]
        public async Task<string> GetEndPoint()
        {
            return await SendRequest(endPointServer, "/book").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> GetMvc()
        {
            return await SendRequest(mvcServer, "/book").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> GetMinimal()
        {
            return await SendRequest(minimalServer, "/book").ConfigureAwait(false);
        }

 
        private static async Task<string> SendRequest(HttpClient httpClient, string route = "/test1")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidEndpointSetupException("Have not setup the test endpoints correctly");
            var output = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return output;
        }

    }
}
