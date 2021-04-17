namespace Easy.Endpoints
{

    public class PutAttribute : EndpointMethodAttribute
    {
        public PutAttribute() : this(null)
        {
        }

        public PutAttribute(string? template) : base(new[] { "PUT" }, template)
        {
        }
    }
}
