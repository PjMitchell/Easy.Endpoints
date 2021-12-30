using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Used to handle endpoint JSON serialization 
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Write object to HttpResponse 
        /// </summary>
        /// <param name="response">Response to write to</param>
        /// <param name="value">Value to serialize</param>
        /// <param name="inputType">Value type</param>
        /// <param name="cancellationToken">CancelationToken for operation</param>
        /// <returns>Task repsenting operation</returns>
        Task SerializeToResponse(HttpResponse response, object? value, Type inputType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Write object to HttpResponse 
        /// </summary>
        /// <typeparam name="TInput">Type of Inputed value</typeparam>
        /// <param name="response">Response to write to</param>
        /// <param name="value">Value to serialize</param>
        /// <param name="cancellationToken">CancelationToken for operation</param>
        /// <returns>Task repsenting operation</returns>
        Task SerializeToResponse<TInput>(HttpResponse response, TInput value, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deserializes HttpRequest
        /// </summary>
        /// <param name="request">Request to read from</param>
        /// <param name="outputType">Desired object type</param>
        /// <param name="bindingErrors">Binding error collection to log errors to</param>
        /// <param name="cancellationToken">CancelationToken for operation</param>
        /// <returns>ValueTask with resulting ParameterBindingResult</returns>
        ValueTask<ParameterBindingResult> DeserializeFromRequest(HttpRequest request, Type outputType, IBindingErrorCollection bindingErrors, CancellationToken cancellationToken = default);
    }
}
