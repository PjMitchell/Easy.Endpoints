namespace Easy.Endpoints
{
    public interface IIntIdRouteParser
    {
        int GetIdFromRoute();
    }

    public class IntIdRouteParser : IIntIdRouteParser
    {
        private readonly IEndpointContextAccessor httpContextAccessor;

        public IntIdRouteParser(IEndpointContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int GetIdFromRoute()
        {
            if (httpContextAccessor.Context is not null && httpContextAccessor.Context.HttpContext.Request.RouteValues.TryGetValue("id", out var value))
            {
                return value switch
                {
                    int i => i,
                    string s => int.Parse(s),
                    _ => 0
                };
            }

            return 0;
        }
    }
}
