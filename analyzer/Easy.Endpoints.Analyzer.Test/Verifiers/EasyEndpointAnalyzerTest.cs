using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.IO;

namespace Easy.Endpoints.Analyzer.Test
{
    public class EasyEndpointAnalyzerTest<TAnalyzer> : CSharpAnalyzerTest<TAnalyzer, XUnitVerifier> where TAnalyzer : DiagnosticAnalyzer, new()
    {
        public EasyEndpointAnalyzerTest()
        {
            var refs = new ReferenceAssemblies("net6.0", new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"), Path.Combine("ref", "net6.0"));
            var package = System.Collections.Immutable.ImmutableArray.Create(new PackageIdentity("Easy.Endpoints.Core", "3.1.0"));
            refs = refs.AddPackages(package);
            ReferenceAssemblies = refs;
        }
    }

    public class EasyEndpointCodeFixTest<TAnalyzer, TCodeFix> : CSharpCodeFixTest<TAnalyzer, TCodeFix, XUnitVerifier> where TAnalyzer : DiagnosticAnalyzer, new() where TCodeFix : CodeFixProvider, new()
    {
        public EasyEndpointCodeFixTest()
        {
            var refs = new ReferenceAssemblies("net6.0", new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"), Path.Combine("ref", "net6.0"));
            var package = System.Collections.Immutable.ImmutableArray.Create(new PackageIdentity("Easy.Endpoints.Core", "3.1.0"));
            refs = refs.AddPackages(package);
            ReferenceAssemblies = refs;
        }
    }
}
