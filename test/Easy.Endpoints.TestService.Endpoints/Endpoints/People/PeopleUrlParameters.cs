using Microsoft.AspNetCore.Http;
using System;

namespace Easy.Endpoints.TestService.Endpoints.People
{
    public class PeopleUrlParameters : UrlParameterModel
    {
        private static readonly Action<PeopleUrlParameters, HttpRequest> binding = UrlParameterBindingHelper.BuildBinder<PeopleUrlParameters>();

        public string[] FirstName { get; set; }
        public string[] Surname { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        public override void BindUrlParameters(HttpRequest request) => binding(this, request);
    }
}
