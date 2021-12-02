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
            //var assembly = System.Collections.Immutable.ImmutableArray.Create(typeof(IEndpoint).Assembly.FullName);
            var refs = new ReferenceAssemblies("net6.0", new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"), Path.Combine("ref", "net6.0")); ;
            var package = System.Collections.Immutable.ImmutableArray.Create(new PackageIdentity("Easy.Endpoints", "3.0.0"));
            refs = refs.AddPackages(package);
            //refs.TargetFramework = "net6";
            ReferenceAssemblies = refs;
        }
    }

    public class EasyEndpointCodeFixTest<TAnalyzer, TCodeFix> : CSharpCodeFixTest<TAnalyzer, TCodeFix, XUnitVerifier> where TAnalyzer : DiagnosticAnalyzer, new() where TCodeFix : CodeFixProvider, new()
    {
        public EasyEndpointCodeFixTest()
        {
            //var assembly = System.Collections.Immutable.ImmutableArray.Create(typeof(IEndpoint).Assembly.FullName);
            var refs = new ReferenceAssemblies("net6.0", new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"), Path.Combine("ref", "net6.0")); ;
            var package = System.Collections.Immutable.ImmutableArray.Create(new PackageIdentity("Easy.Endpoints", "3.0.0"));
            refs = refs.AddPackages(package);
            //refs.TargetFramework = "net6";
            ReferenceAssemblies = refs;
        }
    }
}
