namespace Easy.Endpoints
{
    /// <summary>
    /// Tags an IEndpointHandler to show that it expects to return a JSON response
    /// </summary>
    /// <typeparam name="TApiResponse">Model of the response body</typeparam>
    public interface IJsonResponse<TApiResponse>
    {
    }
}
