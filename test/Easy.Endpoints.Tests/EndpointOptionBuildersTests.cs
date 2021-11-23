using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class EndpointOptionBuildersTests
    {
        [Fact]
        public void Defaults()
        {
            var option = new EndpointOptionBuilders().BuildOption();
            Assert.Equal("[endpoint]", option.RoutePattern);
            var singleMetaDeclaration = Assert.Single(option.EndpointMetaDeclarations);
            Assert.IsType<AuthEndpointMetaDataDeclaration>(singleMetaDeclaration);

            Assert.Equal(JsonNamingPolicy.CamelCase, option.JsonSerializerOptions.PropertyNamingPolicy);
            Assert.True(option.JsonSerializerOptions.PropertyNameCaseInsensitive);
        }

        [Fact]
        public void WithRoutePattern()
        {
            var testPattern = "test";
            var option = new EndpointOptionBuilders().WithRoutePattern(testPattern)
                .BuildOption();
            Assert.Equal(testPattern, option.RoutePattern);
        }

        [Fact]
        public void With_NewJsonOption()
        {
            var jsonOption = new JsonSerializerOptions();
            var option = new EndpointOptionBuilders().WithJsonSerializer(jsonOption)
                .BuildOption();
            Assert.Equal(jsonOption, option.JsonSerializerOptions);
        }

        [Fact]
        public void With_ModifiedJsonOption()
        {
            var option = new EndpointOptionBuilders().WithJsonSerializer(o => o.PropertyNameCaseInsensitive = false)
                .BuildOption();
            Assert.Equal(JsonNamingPolicy.CamelCase, option.JsonSerializerOptions.PropertyNamingPolicy);
            Assert.False(option.JsonSerializerOptions.PropertyNameCaseInsensitive);
        }

        [Fact]
        public void With_MetaDeclarations()
        {
            var declarations = new List<IEndpointMetaDataDeclaration> { Mock.Of<IEndpointMetaDataDeclaration>() };
            var option = new EndpointOptionBuilders().WithMetaDataDeclarations(declarations)
                .BuildOption();
            Assert.Equal(declarations, option.EndpointMetaDeclarations);
        }

        [Fact]
        public void With_ModifiedMetaDeclarations()
        {
            var additional = Mock.Of<IEndpointMetaDataDeclaration>();
            var option = new EndpointOptionBuilders().WithMetaDataDeclarations(o => o.Concat(new[] { additional }))
                .BuildOption();
            Assert.Equal(2, option.EndpointMetaDeclarations.Count);
            Assert.Contains(option.EndpointMetaDeclarations,a=> a == additional);

        }
    }
}
