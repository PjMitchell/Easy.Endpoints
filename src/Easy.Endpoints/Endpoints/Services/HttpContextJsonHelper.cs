using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public static class HttpContextJsonHelper
    {
        public static Task WriteJsonResponse<TModel>(HttpContext httpContext, TModel response, int statusCode = 200)
        {
            httpContext.Response.StatusCode = statusCode;
            return httpContext.Response.WriteAsJsonAsync(response);
        }

        public static ValueTask<TModel?> ReadJsonBody<TModel>(HttpContext httpContext)
        {
            return httpContext.Request.ReadFromJsonAsync<TModel>();
        }
    }
}
