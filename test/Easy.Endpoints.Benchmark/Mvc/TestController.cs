using Microsoft.AspNetCore.Mvc;

namespace Easy.Endpoints.Benchmark.Mvc
{
    public class TestController : Controller
    {
        [HttpGet("test1")]
        public TestResponsePayload Get()
        {
            return TestResponsePayload.Default;
        }
    }
}
