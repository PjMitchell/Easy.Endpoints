namespace Easy.Endpoints
{

    public class PatchAttribute : EndpointMethodAttribute
    {
        public PatchAttribute() : this(null)
        {
        }

        public PatchAttribute(string? template) : base(new[] { "PATCH" }, template)
        {
        }
    }
}
