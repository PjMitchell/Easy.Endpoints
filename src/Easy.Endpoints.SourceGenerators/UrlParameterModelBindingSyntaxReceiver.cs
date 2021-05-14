using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints.SourceGenerators
{
    /// <summary>
    /// Syntax receiver for UrlParameterModelBinding
    /// </summary>
    public class UrlParameterModelBindingSyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// Creates new instance of UrlParameterModelBindingSyntaxReceiver
        /// </summary>
        public UrlParameterModelBindingSyntaxReceiver()
        {
            ClassesToUpdate = new List<ClassDeclarationSyntax>();
        }

        /// <summary>
        /// Discovered Classes to generate code from
        /// </summary>
        public List<ClassDeclarationSyntax> ClassesToUpdate { get; }

        /// <inheritdoc cref="ISyntaxReceiver.OnVisitSyntaxNode"/>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclaration && IsValidClassDeclaration(classDeclaration))
            {
                ClassesToUpdate.Add(classDeclaration);
            }
        }

        private bool IsValidClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Modifiers.Any(a => a.ValueText == "partial") 
                && classDeclaration.BaseList is not null
                && classDeclaration.BaseList.Types.OfType<SimpleBaseTypeSyntax>().Any(a => a.Type is IdentifierNameSyntax id && id.Identifier.ValueText == "UrlParameterModel");
        }
    }
}