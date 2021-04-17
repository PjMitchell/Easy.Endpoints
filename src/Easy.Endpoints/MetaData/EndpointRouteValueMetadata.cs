namespace Easy.Endpoints
{

    public class EndpointRouteValueMetadata : IEndpointRouteValueMetadataProvider
    {
        public EndpointRouteValueMetadata(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public string Value { get; }
    }
}
