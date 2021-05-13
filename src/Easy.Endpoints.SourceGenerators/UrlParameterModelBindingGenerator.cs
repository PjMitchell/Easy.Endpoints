using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints.SourceGenerators
{
    /// <summary>
    /// Generates implementation for void BindUrlParameters(Microsoft.AspNetCore.Http.HttpRequest request) on partial classes with a base class UrlParameterModel
    /// </summary>
    [Generator]
    public class UrlParameterModelBindingGenerator : ISourceGenerator
    {
        private const string template = @"public partial class {0}
        {{
            public override void BindUrlParameters(Microsoft.AspNetCore.Http.HttpRequest request)
            {{
                {1}
            }}
        }}";

        private const string queryParserTemplate = @"if (request.TryGetQueryParameter({0}, Errors, out {1} _gen{2}))
        {2} = _gen{2};
        ";

        private const string routeParserTemplate = @"if (request.TryGetRouteParameter({0}, out {1} _gen{2}))
        {{
            {2} = _gen{2};
        }}
        else
        {{
            Errors.Add(new UrlParameterModelError({0}, string.Format(UrlParameterErrorMessages.InvalidRouteParameterError, {0})));
        }}
        ";

        private readonly HashSet<string> availablePrimatives;
        private readonly HashSet<string> availableNullablePrimatives;
        private readonly HashSet<string> availableArrayPrimatives;
        private readonly HashSet<string> availablePropertyIdentifiers;
        private readonly HashSet<string> availableNullablePropertyIdentifiers;
        private readonly HashSet<string> availableArrayPropertyIdentifiers;

        /// <summary>
        /// Creates new instance UrlParameterModelBindingGenerator
        /// </summary>
        public UrlParameterModelBindingGenerator()
        {
            availablePrimatives = new HashSet<string>(new[] { "string", "int", "long", "bool", "double" }, StringComparer.InvariantCulture);
            availableNullablePrimatives = new HashSet<string>(new[] { "int", "long", "bool", "double" }, StringComparer.InvariantCulture);
            availableArrayPrimatives = new HashSet<string>(new[] { "string", "int", "long", "double" }, StringComparer.InvariantCulture);

            availablePropertyIdentifiers = new HashSet<string>(new[] { "Guid", "DateTime", "DateTimeOffset" }, StringComparer.InvariantCulture);
            availableNullablePropertyIdentifiers = new HashSet<string>(new[] { "Guid", "DateTime", "DateTimeOffset" }, StringComparer.InvariantCulture);
            availableArrayPropertyIdentifiers = new HashSet<string>(new[] { "Guid" }, StringComparer.InvariantCulture);
        }

        private string WrapCodeWithUsings(ClassDeclarationSyntax model, string classDeclaration)
        {
            var parentNode = model.SyntaxTree.GetRoot();
            var usingBlock = string.Join(Environment.NewLine, parentNode.ChildNodes().OfType<UsingDirectiveSyntax>().Select(s => s.ToString()));
            var namespaceElement = parentNode.ChildNodes().OfType<NamespaceDeclarationSyntax>().Single();
            var namespaceName = namespaceElement.Name.ToString();
            var subNameSpaceUsing = string.Join(Environment.NewLine, namespaceElement.ChildNodes().OfType<UsingDirectiveSyntax>().Select(s => s.ToString()));
            return @$"
        {usingBlock}
        namespace {namespaceName}
        {{
            {subNameSpaceUsing}
            {classDeclaration}
        }}
        ";
        }

        /// <inheritdoc cref="ISourceGenerator.Execute"/>
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (UrlParameterModelBindingSyntaxReceiver)context.SyntaxReceiver;
            foreach (var model in syntaxReceiver.ClassesToUpdate)
            {
                var bindingCode = BindingCode(model);
                var classDeclaration = string.Format(template, model.Identifier.Text, bindingCode);
                var source = WrapCodeWithUsings(model, classDeclaration);context.AddSource($"{model.Identifier.Text}.gen.cs", source);
            }
        }


        private string BindingCode(ClassDeclarationSyntax model)
        {
            return string.Join(Environment.NewLine, BindingLines(model));
        }

        private IEnumerable<string> BindingLines(ClassDeclarationSyntax model)
        {
            foreach (var member in model.Members.OfType<PropertyDeclarationSyntax>())
            {
                if (IsRouteParameter(member))
                {
                    if (TryRouteDeclaration(member, out var routeString))
                        yield return routeString;
                }
                else if (TryQueryDeclaration(member, out var queryString))
                    yield return queryString;
            }
        }

        /// <inheritdoc cref="ISourceGenerator.Initialize"/>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new UrlParameterModelBindingSyntaxReceiver());
        }

        private bool TryQueryDeclaration(PropertyDeclarationSyntax propertyDeclaration, out string result)
        {
            if (TryGetTypeText(propertyDeclaration, out var typeString))
            {
                var parameter = GetQueryParameterText(propertyDeclaration);
                result = string.Format(queryParserTemplate, parameter, typeString, propertyDeclaration.Identifier.ValueText);
                return true;
            }

            result = string.Empty;
            return false;
        }

        private bool TryRouteDeclaration(PropertyDeclarationSyntax propertyDeclaration, out string result)
        {
            if (TryGetTypeText(propertyDeclaration, out var typeString, true))
            {
                var parameter = GetRouteParameterText(propertyDeclaration);
                result = string.Format(routeParserTemplate, parameter, typeString, propertyDeclaration.Identifier.ValueText);
                return true;
            }

            result = string.Empty;
            return false;
        }

        private bool TryGetTypeText(PropertyDeclarationSyntax propertyDeclaration, out string result, bool routeTypes = false)
        {
            if (!routeTypes && propertyDeclaration.Type is ArrayTypeSyntax arrayType)
            {
                if (arrayType.ElementType is PredefinedTypeSyntax predefinedType && (availableArrayPrimatives.Contains(predefinedType.Keyword.ValueText)))
                {
                    result = $"{predefinedType.Keyword.ValueText}[]";
                    return true;
                }
                if (arrayType.ElementType is IdentifierNameSyntax identifierNameSyntax && availableArrayPropertyIdentifiers.Contains(identifierNameSyntax.Identifier.ValueText))
                {
                    result = $"{identifierNameSyntax.Identifier.ValueText}[]";
                    return true;
                }
            }
            else if (!routeTypes && propertyDeclaration.Type is NullableTypeSyntax nullableType)
            {
                if (nullableType.ElementType is PredefinedTypeSyntax predefinedType && (availableNullablePrimatives.Contains(predefinedType.Keyword.ValueText)))
                {
                    result = $"{predefinedType.Keyword.ValueText}?";
                    return true;
                }
                if (nullableType.ElementType is IdentifierNameSyntax identifierNameSyntax && availableNullablePropertyIdentifiers.Contains(identifierNameSyntax.Identifier.ValueText))
                {
                    result = $"{identifierNameSyntax.Identifier.ValueText}?";
                    return true;
                }
            }
            else if (propertyDeclaration.Type is PredefinedTypeSyntax predefinedTypeSyntax && availablePrimatives.Contains(predefinedTypeSyntax.Keyword.ValueText))
            {
                result = predefinedTypeSyntax.Keyword.ValueText;
                return true;
            }
            else if (propertyDeclaration.Type is IdentifierNameSyntax identifierNameSyntax && availablePropertyIdentifiers.Contains(identifierNameSyntax.Identifier.ValueText))
            {
                result = identifierNameSyntax.Identifier.ValueText;
                return true;
            }

            result = string.Empty;
            return false;
        }

        private static string GetQueryParameterText(PropertyDeclarationSyntax propertyDeclaration)
        {
            var queryAttribute = propertyDeclaration.AttributeLists.SelectMany(s => s.Attributes).FirstOrDefault(r => r.Name is IdentifierNameSyntax identifier && identifier.Identifier.Text == "QueryParameter");
            if (queryAttribute is not null)
            {
                if (queryAttribute.ArgumentList.Arguments.Single().Expression is LiteralExpressionSyntax literalExpressions)
                {
                    return literalExpressions.Token.Text;
                }
                else if (queryAttribute.ArgumentList.Arguments.Single().Expression is IdentifierNameSyntax identifier)
                {
                    return identifier.Identifier.ValueText;
                }
            }
            return $"\"{GetParameterName(propertyDeclaration.Identifier.ValueText)}\"";
        }

        private static bool IsRouteParameter(PropertyDeclarationSyntax propertyDeclaration)
        {
            return propertyDeclaration.AttributeLists.SelectMany(s => s.Attributes).Any(r => r.Name is IdentifierNameSyntax identifier && identifier.Identifier.Text == "RouteParameter");
        }

        private static string GetRouteParameterText(PropertyDeclarationSyntax propertyDeclaration)
        {
            var queryAttribute = propertyDeclaration.AttributeLists.SelectMany(s => s.Attributes).FirstOrDefault(r => r.Name is IdentifierNameSyntax identifier && identifier.Identifier.Text == "RouteParameter");
            if (queryAttribute is not null && queryAttribute.ArgumentList.Arguments.Count == 1)
            {
                if (queryAttribute.ArgumentList.Arguments.Single().Expression is LiteralExpressionSyntax literalExpressions)
                {
                    return literalExpressions.Token.Text;
                }
                else if (queryAttribute.ArgumentList.Arguments.Single().Expression is IdentifierNameSyntax identifier)
                {
                    return identifier.Identifier.ValueText;
                }
            }
            return $"\"{GetParameterName(propertyDeclaration.Identifier.ValueText)}\"";
        }

        private static string GetParameterName(string propertyName)
        {
            var characters = propertyName.ToCharArray();
            characters[0] = char.ToLower(characters[0]);
            return new string(characters);
        }
    }
}