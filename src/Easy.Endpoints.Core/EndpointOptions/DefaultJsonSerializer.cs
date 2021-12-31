using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Default IJsonSerializer using System.Text.Json
    /// </summary>
    public class DefaultJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions serializerOptions;

        /// <summary>
        /// Gets Default JsonSerializerOptions
        /// </summary>
        /// <returns>Default JsonSerializerOptions</returns>
        public static JsonSerializerOptions GetDefaultSettings()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
        }

        /// <summary>
        /// New instance of DefaultJsonSerializer
        /// </summary>
        /// <param name="serializerOptions">Options</param>
        public DefaultJsonSerializer(JsonSerializerOptions serializerOptions)
        {
            this.serializerOptions = serializerOptions;
        }

        /// <summary>
        /// New instance of DefaultJsonSerializer with default options
        /// </summary>
        public DefaultJsonSerializer()
        {
            serializerOptions = GetDefaultSettings();
        }

        /// <inheritdoc/>
        public async ValueTask<ParameterBindingResult> DeserializeFromRequest(HttpRequest request, Type outputType, IBindingErrorCollection bindingErrors, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await request.ReadFromJsonAsync(outputType, serializerOptions, cancellationToken).ConfigureAwait(false);
                if (result is null)
                {
                    bindingErrors.Add(new BindingError("body", "could not parse body"));
                    return new ParameterBindingResult(null, ParameterBindingIssues.Error);
                }
                return new ParameterBindingResult(result);
            }
            catch (JsonException e)
            {
                bindingErrors.Add(new BindingError("body", $"could not parse body; {e.Message}"));
                return new ParameterBindingResult(null, ParameterBindingIssues.Error);
            }
        }
        /// <inheritdoc/>
        public Task SerializeToResponse(HttpResponse response, object? value, Type inputType, CancellationToken cancellationToken = default)
        {
            response.ContentType = "application/json; charset = utf-8";
            return JsonSerializer.SerializeAsync(response.Body, value, inputType, serializerOptions, cancellationToken);
        }
        /// <inheritdoc/>
        public Task SerializeToResponse<TInput>(HttpResponse response, TInput value, CancellationToken cancellationToken = default)
        {
            response.ContentType = "application/json; charset = utf-8";
            return JsonSerializer.SerializeAsync(response.Body, value, serializerOptions, cancellationToken);
        }
    }
}
