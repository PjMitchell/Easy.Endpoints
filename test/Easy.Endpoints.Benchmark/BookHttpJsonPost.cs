using BenchmarkDotNet.Attributes;
using Easy.Endpoints.TestService.Endpoints;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark
{
    [RankColumn, MemoryDiagnoser]
    public class BookHttpJsonPost
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
        public async Task<string> PostEndPoint()
        {
            return await SendRequest(endPointServer, "/book").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> PostMvc()
        {
            return await SendRequest(mvcServer, "/book").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> PostMinimal()
        {
            return await SendRequest(minimalServer, "/book").ConfigureAwait(false);
        }

        private static async Task<string> SendRequest(HttpClient httpClient, string route)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, route)
            {
                Content = new StringContent(JsonSerializer.Serialize(new Book { Id = 23, Name = "Test" }), Encoding.UTF8, "application/json")
            };
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            var output = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return output;
        }

    }
}
