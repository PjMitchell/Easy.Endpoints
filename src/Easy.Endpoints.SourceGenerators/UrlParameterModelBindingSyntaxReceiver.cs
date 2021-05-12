using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints.SourceGenerators
{

    public class UrlParameterModelBindingSyntaxReceiver : ISyntaxReceiver
    {
        public UrlParameterModelBindingSyntaxReceiver()
        {
            ClassesToUpdate = new List<ClassDeclarationSyntax>();
        }

        public List<ClassDeclarationSyntax> ClassesToUpdate { get; }

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