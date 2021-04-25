namespace Easy.Endpoints
{
    /// <summary>
    /// Tags an IEndpointHandler to show that it expects a JSON body
    /// </summary>
    /// <typeparam name="TApiBody">Model for the JSON Body</typeparam>
    public interface IJsonBody<TApiBody>
    {
    }
}
