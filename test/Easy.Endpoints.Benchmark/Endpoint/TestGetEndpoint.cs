using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark.Endpoint
{
    [Get("test1")]
    public class TestGetEndpoint : IJsonResponseEndpointHandler<TestResponsePayload>
    {
        public Task<TestResponsePayload> Handle(CancellationToken cancellationToken) => Task.FromResult(TestResponsePayload.Default);
    }

    [Get("test2")]
    public class Test2GetEndpoint : IJsonResponseEndpointHandler<TestResponsePayload>
    {
        public Task<TestResponsePayload> Handle(CancellationToken cancellationToken) => Task.FromResult(TestResponsePayload.Default);
    }
}
