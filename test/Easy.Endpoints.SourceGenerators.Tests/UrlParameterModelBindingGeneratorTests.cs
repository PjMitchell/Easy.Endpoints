using Easy.Endpoints.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class UrlParameterModelBindingGeneratorTests
    {
        [Fact]
        public void TestUrlParameteModelBindingGenerator()
        {
            
            var inputCompilation = CreateCompilation(TestTestCodeGeneratedUrlParametersCode.ValidUrlParameterModel);
            var generator = new UrlParameterModelBindingGenerator();
            GeneratorDriver generatorDriver = CSharpGeneratorDriver.Create(generator);
            generatorDriver = generatorDriver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            Assert.Empty(diagnostics);
            Assert.Equal(2, outputCompilation.SyntaxTrees.Count());
            Assert.Empty(outputCompilation.GetDiagnostics());
            GeneratorDriverRunResult runResult = generatorDriver.GetRunResult();
            Assert.Single(runResult.GeneratedTrees);
            Assert.Empty(runResult.Diagnostics);

            GeneratorRunResult generatorResult = Assert.Single(runResult.Results);
            Assert.Equal(generator, generatorResult.Generator);
            Assert.Empty(generatorResult.Diagnostics);
            var source = Assert.Single(generatorResult.GeneratedSources);
            Assert.Equal("TestCodeGeneratedUrlParameters.gen.cs", source.HintName);
            Assert.Null(generatorResult.Exception);
        }


        private static Compilation CreateCompilation(params string[] source)
            => CSharpCompilation.Create("compilation",
                source.Select(s=> CSharpSyntaxTree.ParseText(s)),
                GetAssemblyWithReferences(typeof(UrlParameterModel).GetTypeInfo().Assembly)
                    .Concat(GetAssemblyWithReferences(Assembly.Load("netstandard")))
                    .Concat(GetAssemblyWithReferences(Assembly.Load("System.Private.CoreLib")))
                    .Select(a => MapToMetaDataReference(a)),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


        private static IEnumerable<Assembly> GetAssemblyWithReferences(Assembly assembly)
        {
            var assemblies = new[] { assembly };

            var referencedAssemblyNames = assembly.GetReferencedAssemblies();

            return assemblies.Concat(referencedAssemblyNames.Select(Assembly.Load));

        }

        private static MetadataReference MapToMetaDataReference(Assembly a)
        {
            return MetadataReference.CreateFromFile(a.Location);
        }
    }

    public static class TestTestCodeGeneratedUrlParametersCode
    {
        public const string ValidUrlParameterModel = @"
            using System;
            using Easy.Endpoints;

            namespace TestNameSpace
            {

                public partial class TestCodeGeneratedUrlParameters : UrlParameterModel
                {
                    private const string startDateParameter = ""start"";
                    private const string otherRouteParameter = ""otherId"";
                    public string[] FirstName { get; private set; } = Array.Empty<string>();
                    [QueryParameter(""lastname"")]
                    public string Surname { get; set; } = string.Empty;
                    public int MinAge { get; set; }
                    public int? MaxAge { get; set; }
                    [RouteParameter()]
                    public string Route { get; set; } = string.Empty;
                    [QueryParameter(startDateParameter)]
                    public DateTime? StartDate { get; set; }
                    [RouteParameter(""id"")]
                    public Guid UniqueId { get; set; } = Guid.Empty;
                    [RouteParameter(otherRouteParameter)]
                    public Guid Other { get; set; } = Guid.Empty;
                    public Guid[] OtherIds { get; set; } = Array.Empty<Guid>();

                }
            }
            ";

    }
}
