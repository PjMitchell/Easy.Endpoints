using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class HttpContextJsonHelper
    {
        public static Task WriteJsonResponse<TModel>(HttpContext httpContext, TModel response, int statusCode = 200)
        {
            httpContext.Response.StatusCode = statusCode;
            return httpContext.Response.WriteAsJsonAsync(response);
        }

        public static async ValueTask<TModel> ReadJsonBody<TModel>(HttpContext httpContext)
        {
            try
            {
                var result = await httpContext.Request.ReadFromJsonAsync<TModel>();
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
