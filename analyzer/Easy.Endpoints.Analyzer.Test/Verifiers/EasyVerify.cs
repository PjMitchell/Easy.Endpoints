using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Easy.Endpoints.Analyzer.Test
{
    public class EasyVerify<TAnalyzer> : AnalyzerVerifier<TAnalyzer, EasyEndpointAnalyzerTest<TAnalyzer>, XUnitVerifier> where TAnalyzer : DiagnosticAnalyzer, new() 
    {
        public EasyVerify()
        {
            
        }
    }

    public class EasyVerify<TAnalyzer, TCodeFix> : CodeFixVerifier<TAnalyzer,TCodeFix, EasyEndpointCodeFixTest<TAnalyzer, TCodeFix>, XUnitVerifier> where TAnalyzer : DiagnosticAnalyzer, new() where TCodeFix : CodeFixProvider, new ()
    {
        public EasyVerify()
        {

        }
    }
}
