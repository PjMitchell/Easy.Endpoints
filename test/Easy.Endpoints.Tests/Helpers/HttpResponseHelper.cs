using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Easy.Endpoints.Tests
{
    public static class HttpResponseHelper
    {
        public static async Task<T> GetJsonBody<T>(this HttpResponseMessage message)
        {
            var resultBody = await message.Content.ReadAsStringAsync();
            var observed = JsonSerializer.Deserialize<T>(resultBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return observed;
        }
    }
}
