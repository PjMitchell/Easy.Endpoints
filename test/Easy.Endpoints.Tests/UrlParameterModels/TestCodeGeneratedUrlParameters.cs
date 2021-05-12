using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public partial class TestCodeGeneratedUrlParameters : UrlParameterModel
    {
        private const string startDateParameter = "start";
        public string[] FirstName { get; private set; } = Array.Empty<string>();
        [QueryParameter("lastname")]
        public string Surname { get; set; } = string.Empty;
        public int MinAge { get; set; }
        public int? MaxAge { get; set; }
        [RouteParameter()]
        public string Route { get; set; } = string.Empty;
        [QueryParameter(startDateParameter)]
        public DateTime StartDate { get; set; }
    }

    public class CodeGeneratedUrlParameterTests
    {
        private readonly DateTime one = new DateTime(2019, 10, 1, 9, 13, 42);
        private readonly TestServer server;

        public CodeGeneratedUrlParameterTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<UrlModelEndpoint>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest($"RouteValue/Test?firstName=bob&lastname=par&minAge=1&start={one:yyyy-MM-ddTHH:mm:ss}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<TestUrlParameterEndpointResult<TestCodeGeneratedUrlParameters>>();
            Assert.Empty(observed.Errors);

        }

        [Get("{route}/Test")]
        public class UrlModelEndpoint : TestUrlParameterEndpoint<TestCodeGeneratedUrlParameters> { }
    }
}
