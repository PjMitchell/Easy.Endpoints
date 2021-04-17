namespace Easy.Endpoints
{

    public class DeleteAttribute : EndpointMethodAttribute
    {
        public DeleteAttribute() : this(null)
        {
        }

        public DeleteAttribute(string? template) : base(new[] { "DELETE" }, template)
        {
        }
    }
}
