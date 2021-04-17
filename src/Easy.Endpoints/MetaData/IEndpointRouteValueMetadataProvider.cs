using Microsoft.AspNetCore.Mvc.Filters;

namespace Easy.Endpoints
{

    public interface IEndpointRouteValueMetadataProvider : IFilterMetadata
    {
        string Key { get; }
        string Value { get; }
    }
}
