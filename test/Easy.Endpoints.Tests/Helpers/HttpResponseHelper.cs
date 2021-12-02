using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public static class HttpResponseHelper
    {
        public static async Task<T> GetJsonBody<T>(this HttpResponseMessage message)
        {
            var resultBody = await message.Content.ReadAsStringAsync();
            var settings = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            settings.Converters.Add(new DateOnlyJsonConverter());
            settings.Converters.Add(new TimeOnlyJsonConverter());
            var observed = JsonSerializer.Deserialize<T>(resultBody, settings);
            Assert.NotNull(observed);
#pragma warning disable CS8603 // Possible null reference return.
            return observed;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
