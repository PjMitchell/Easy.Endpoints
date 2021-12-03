using Microsoft.AspNetCore.Http;
using System;
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

        public static Task WriteJsonResponse<TModel>(HttpContext endpointContext, JsonSerializerOptions serializerOptions, TModel response, int statusCode = 200)
        {
            endpointContext.Response.StatusCode = statusCode;
            endpointContext.Response.ContentType = "application/json; charset = utf-8";
            return JsonSerializer.SerializeAsync(endpointContext.Response.Body, response, typeof(TModel), serializerOptions, endpointContext.RequestAborted);
        }

        public static Task WriteJsonResponse(HttpContext endpointContext,JsonSerializerOptions serializerOptions, object response,Type inputType,  int statusCode = 200)
        {
            endpointContext.Response.StatusCode = statusCode;
            endpointContext.Response.ContentType = "application/json; charset = utf-8";
            return JsonSerializer.SerializeAsync(endpointContext.Response.Body, response, inputType, serializerOptions, endpointContext.RequestAborted);
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

        public static async ValueTask<object?> ReadJsonBody(HttpContext httpContext,Type type, JsonSerializerOptions serializerOptions)
        {
            try
            {
                var result = await httpContext.Request.ReadFromJsonAsync(type, serializerOptions, httpContext.RequestAborted).ConfigureAwait(false);
                if (result is null)
                    throw new EndpointStatusCodeResponseException(400, "Invalid request body");
                return result;
            }
            catch (JsonException)
            {
                throw new EndpointStatusCodeResponseException(400, "Invalid request body");
            }
        }
    }
}
