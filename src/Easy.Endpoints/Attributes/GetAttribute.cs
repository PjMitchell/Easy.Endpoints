namespace Easy.Endpoints
{

    public class GetAttribute : EndpointMethodAttribute
    {
        public GetAttribute() : this(null)
        {
        }

        public GetAttribute(string? template) : base(new[] { "GET" }, template)
        {
        }
    }
}
