using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EasyEndpointAnalyzerCodeFixProvider)), Shared]
    public class EasyEndpointAnalyzerCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(EasyEndpointWarnings.AsyncForHandleId, EasyEndpointWarnings.SyncForHandleAsyncId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            foreach(var diagnostic in context.Diagnostics)
                RegisterForDiagnostic(context, diagnostic, root);
        }

        private void RegisterForDiagnostic(CodeFixContext context, Diagnostic diagnostic, SyntaxNode root)
        {
            switch (diagnostic.Id)
            {
                case EasyEndpointWarnings.AsyncForHandleId:
                    RegisterForAsyncForHandleDiagnostic(context, diagnostic, root);
                    break;
                case EasyEndpointWarnings.SyncForHandleAsyncId:
                    RegisterForSyncForHandleAsyncDiagnostic(context, diagnostic, root);
                    break;

            }
        }

        private void RegisterForAsyncForHandleDiagnostic(CodeFixContext context, Diagnostic diagnostic, SyntaxNode root)
        {
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var declaration = root?.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.FixAsyncForHandle,
                    createChangedSolution: c => RenameHandleToHandleAsyncAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.FixAsyncForHandle)),
                diagnostic);
        }

        private void RegisterForSyncForHandleAsyncDiagnostic(CodeFixContext context, Diagnostic diagnostic, SyntaxNode root)
        {
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var declaration = root?.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.FixSyncForHandleAsync,
                    createChangedSolution: c => RenameHandleAsyncToHandleAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.FixSyncForHandleAsync)),
                diagnostic);
        }


        private async Task<Solution> RenameHandleAsyncToHandleAsync(Document document, MethodDeclarationSyntax methodDeclarationSyntax, CancellationToken cancellationToken)
        {
            var newName = "Handle";

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclarationSyntax, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, methodSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }

        private async Task<Solution> RenameHandleToHandleAsyncAsync(Document document, MethodDeclarationSyntax methodDeclarationSyntax, CancellationToken cancellationToken)
        {
            var newName = "HandleAsync";

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclarationSyntax, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, methodSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }
    }
}
