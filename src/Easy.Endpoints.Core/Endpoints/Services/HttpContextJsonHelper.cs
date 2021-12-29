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

        public static async ValueTask<ParameterBindingResult> ReadJsonBody(HttpContext httpContext,Type type, JsonSerializerOptions serializerOptions, IBindingErrorCollection bindingErrors)
        {
            try
            {
                var result = await httpContext.Request.ReadFromJsonAsync(type, serializerOptions, httpContext.RequestAborted).ConfigureAwait(false);
                if (result is null)
                {
                    bindingErrors.Add(new BindingError("body", "could not parse body"));
                    return new ParameterBindingResult(null, ParameterBindingFlag.Error);
                }
                return new ParameterBindingResult(result);
            }
            catch (JsonException e)
            {
                bindingErrors.Add(new BindingError("body", $"could not parse body; {e.Message}"));
                return new ParameterBindingResult(null, ParameterBindingFlag.Error);
            }
        }
    }
}
