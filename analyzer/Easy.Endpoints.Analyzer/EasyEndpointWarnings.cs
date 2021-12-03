using Microsoft.CodeAnalysis;
using static Easy.Endpoints.Analyzer.Resources;

namespace Easy.Endpoints.Analyzer
{
    public class EasyEndpointWarnings
    {
        public const string EmptyId = "EE1001";
        public const string MultipleId = "EE1001";
        public const string SyncForHandleAsyncId = "EE1101";
        public const string AsyncForHandleId = "EE1102";


        public static readonly DiagnosticDescriptor Empty = UsageDescriptor("EE1001", nameof(EmptyEndpointTitle), nameof(EmptyEndpointMessageFormat), nameof(EmptyEndpointDescription));
        public static readonly DiagnosticDescriptor Multiple = UsageDescriptor("EE1002", nameof(MultipleHandlersForEndpointTitle), nameof(MultipleHandlersForEndpointMessageFormat), nameof(MultipleHandlersForEndpointDescription));
        public static readonly DiagnosticDescriptor SyncForHandleAsync = NamingDescriptor("EE1101", nameof(AsyncHandlersForSyncEndpointTitle), nameof(AsyncHandlersForSyncEndpointMessageFormat), nameof(AsyncHandlersForSyncEndpointDescription));
        public static readonly DiagnosticDescriptor AsyncForHandle = NamingDescriptor("EE1102", nameof(SyncHandlersForAsyncEndpointTitle), nameof(SyncHandlersForAsyncEndpointMessageFormat), nameof(SyncHandlersForAsyncEndpointDescription));


        private static DiagnosticDescriptor Descriptor(string code, string title, string messageFormat, string description, string category) => new DiagnosticDescriptor(code, Localize(title), Localize(messageFormat), category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Localize(description));
        private static DiagnosticDescriptor UsageDescriptor(string code, string title, string messageFormat, string description) => Descriptor(code, title, messageFormat, description, "Usage");


        private static DiagnosticDescriptor NamingDescriptor(string code, string title, string messageFormat, string description) => Descriptor(code, title, messageFormat, description, "Naming");
        private static LocalizableString Localize(string resource) => new LocalizableResourceString(resource, ResourceManager, typeof(Resources));
    }
}
