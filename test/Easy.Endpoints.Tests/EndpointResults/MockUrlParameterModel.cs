using Microsoft.AspNetCore.Http;

namespace Easy.Endpoints.Tests
{
    public class MockUrlParameterModel : UrlParameterModel
    {
        public const string TestQueryParameter = "testValue";
        public const string ErrorMessage = "Nope";


        public override void BindUrlParameters(HttpRequest request)
        {
            if(request.Query.TryGetValue(TestQueryParameter, out var strings) && strings.Count ==1)
            {
                TestValue = strings[0];
            }
            else
            {
                Errors.Add(new UrlParameterModelError(TestQueryParameter, ErrorMessage));
            }
        }

        public string TestValue { get; private set; } = string.Empty;   
    }

    public class MockJsonBodyModel
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
