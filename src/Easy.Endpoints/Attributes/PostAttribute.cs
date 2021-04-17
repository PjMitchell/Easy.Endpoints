namespace Easy.Endpoints
{

    public class PostAttribute : EndpointMethodAttribute
    {
        public PostAttribute() : this(null)
        {
        }

        public PostAttribute(string? template) : base(new[] { "POST" }, template)
        {
        }
    }
}
