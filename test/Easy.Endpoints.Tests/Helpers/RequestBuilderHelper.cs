using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Easy.Endpoints.Tests
{
    public static class RequestBuilderHelper
    {
        public static RequestBuilder AndJsonBody<TBody>(this RequestBuilder source, TBody body)
        {
            var json = JsonSerializer.Serialize(body);
            return source.And(v => v.Content = new StringContent(json, Encoding.UTF8, "application/json"));
        }
    }
}
