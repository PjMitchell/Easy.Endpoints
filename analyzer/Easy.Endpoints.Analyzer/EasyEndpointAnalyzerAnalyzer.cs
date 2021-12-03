using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using static Easy.Endpoints.Analyzer.EasyEndpointWarnings;

namespace Easy.Endpoints.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EasyEndpointAnalyzerAnalyzer : DiagnosticAnalyzer
    {       

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Empty, Multiple, AsyncForHandle, SyncForHandleAsync); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var classSymbol = (ClassDeclarationSyntax)context.Node;
            var hasIEndpointInterface = HasIEndpointInterface(context, classSymbol);

            if (!hasIEndpointInterface)
                return;
            var handleMethods = classSymbol.Members.OfType<MethodDeclarationSyntax>().Where(IsHandleMethod).ToArray();

            switch (handleMethods.Length)
            {
                case 0:
                    HandleEmpty(context, classSymbol);
                    break;
                case 1:
                    HandleSingleHandler(context, handleMethods[0]);
                    break;
                default:
                    HandleMultiple(context, classSymbol, handleMethods);
                    break;
            }
        }

        private static void HandleSingleHandler(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax handleMethod)
        {
            var returnType = context.SemanticModel.GetTypeInfo(handleMethod.ReturnType);
            var returnTypeIsAsync = ReturnTypeIsAsync(returnType);
            if (handleMethod.Identifier.ValueText == "HandleAsync" && !returnTypeIsAsync)
            {
                var diagnostic = Diagnostic.Create(SyncForHandleAsync, handleMethod.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
            if (handleMethod.Identifier.ValueText == "Handle" && returnTypeIsAsync)
            {
                var diagnostic = Diagnostic.Create(AsyncForHandle, handleMethod.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
            
        }

        private static bool ReturnTypeIsAsync(TypeInfo typeInfo)
        {
            return typeInfo.Type != null && typeInfo.Type.ContainingAssembly.Identity.Name == "System.Runtime" 
                && (typeInfo.Type.Name == "Task" || typeInfo.Type.Name == "ValueTask");
        }

        private static void HandleMultiple(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classSyntax, MethodDeclarationSyntax[] handleMethods)
        {
            foreach (var handleMethod in handleMethods)
            {
                var diagnostic = Diagnostic.Create(Multiple, handleMethod.GetLocation(), classSyntax.Identifier.Text, handleMethods.Length);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void HandleEmpty(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classSyntax)
        {
            var diagnostic = Diagnostic.Create(Empty, classSyntax.Identifier.GetLocation(), classSyntax.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }

        private static bool HasIEndpointInterface(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclarationSyntax)
        {
            return classDeclarationSyntax?.BaseList?.Types.Select(s => context.SemanticModel.GetTypeInfo(s.Type)).Any(x => IsIEndpoint(x)) ?? false;
        }

        private static bool IsIEndpoint(TypeInfo typeInfo)
        {
            return typeInfo.Type != null && typeInfo.Type.Name == "IEndpoint" && typeInfo.Type.ContainingAssembly.Identity.Name == "Easy.Endpoints.Core";
        }

        private static bool IsHandleMethod(MethodDeclarationSyntax method)
        {
            return method.Identifier.ValueText == "Handle"
                || method.Identifier.ValueText == "HandleAsync"
                && method.Modifiers.Any(m => m.Text == "public");
        }
    }
}
