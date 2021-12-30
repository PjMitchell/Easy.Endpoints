using BenchmarkDotNet.Attributes;
using System.Net.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark
{
    [RankColumn, MemoryDiagnoser]
    public class PeopleQueryHttpJsonGet
    {
        private HttpClient endPointServer;
        private HttpClient mvcServer;


        [GlobalSetup]
        public void Setup()
        {
            endPointServer = ServerFactory.CreateEndpointServer().CreateClient();
            mvcServer = ServerFactory.CreateMvcServer().CreateClient();
        }

        [Benchmark]
        public async Task<string> GetEndPoint()
        {
            return await SendRequest(endPointServer).ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> GetMvc()
        {
            return await SendRequest(mvcServer).ConfigureAwait(false);
        }
 
        private static async Task<string> SendRequest(HttpClient httpClient)
        {
            var route = "/people?minAge=50";
            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidEndpointSetupException("Have not setup the test endpoints correctly");
            var output = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return output;
        }

    }
}
