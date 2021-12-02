using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Analyzer.Test
{
    public class EasyEndpointAnalyzerUnitTest
    {
        [Fact]
        public async Task EmptyCode_ReturnsNoErrors()
        {
            var test = @"";

            await EasyVerify<EasyEndpointAnalyzerAnalyzer>.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task NoValidMethod_ReturnsNoHandlerMethod()
        {
            var test = @"
    using Easy.Endpoints;
    
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public void Nope()
            {
            }
        }
    }";



            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.Empty)
                .WithLocation(6,22)
                .WithArguments("GetTestEndpoint");
            await EasyVerify<EasyEndpointAnalyzerAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }


        [Fact]
        public async Task MultipleValidMethod_ReturnsWarningForEach()
        {
            var test = @"
    using Easy.Endpoints;
    
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public void Handle()
            {
            }

            public void Handle(int id)
            {
            }
        }
    }";



            var expected1 = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.Multiple)
                .WithLocation(8, 13)
                .WithArguments("GetTestEndpoint", 2);

            var expected2 = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.Multiple)
                .WithLocation(12, 13)
                .WithArguments("GetTestEndpoint", 2);
            await EasyVerify<EasyEndpointAnalyzerAnalyzer>.VerifyAnalyzerAsync(test, expected1, expected2);
        }
    }
}
