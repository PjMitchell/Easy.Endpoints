using System.Collections.Generic;

namespace Easy.Endpoints.Tests
{

    public record TestUrlParameterEndpointResult<T>(T Result, List<UrlParameterModelError> Errors)
    {
    }
}
