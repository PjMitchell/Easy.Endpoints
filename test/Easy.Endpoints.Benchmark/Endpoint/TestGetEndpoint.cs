using Easy.Endpoints.TestService.Endpoints.People;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark.Endpoint
{

    [Get("test1")]
    public class TestGetEndpoint : IEndpoint
    {
        public Task<TestResponsePayload> HandleAsync(CancellationToken cancellationToken) => Task.FromResult(TestResponsePayload.Default);
    }

    [Get("test2")]
    public class Test2GetEndpoint : IEndpoint
    {
        public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken) => Task.FromResult(EndpointResult.Ok(TestResponsePayload.Default));
    }
}
