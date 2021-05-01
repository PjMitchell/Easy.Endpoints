using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class HttpContextJsonHelper
    {
        public static Task WriteJsonResponse<TModel>(EndpointContext endpointContext, TModel response, int statusCode = 200)
        {
            endpointContext.Response.StatusCode = statusCode;
            endpointContext.Response.ContentType = "application/json; charset = utf-8";
            return JsonSerializer.SerializeAsync(endpointContext.Response.Body, response, typeof(TModel), endpointContext.JsonSerializerOptions, endpointContext.RequestAborted);
        }

        public static async ValueTask<TModel> ReadJsonBody<TModel>(EndpointContext endpointContext)
        {
            try
            {
                var result = await endpointContext.Request.ReadFromJsonAsync<TModel>(endpointContext.JsonSerializerOptions, endpointContext.RequestAborted).ConfigureAwait(false);
                if (result is null)
                    throw new EndpointStatusCodeResponseException(400, "Invalid request body");
                return result;
            }
            catch(JsonException)
            {
                throw new EndpointStatusCodeResponseException(400, "Invalid request body");
            }
        }
    }
}
