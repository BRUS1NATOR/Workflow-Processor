using System.Text.Json.Serialization.Metadata;

namespace WorkflowProcessor.Persistance.Context.Json
{
    public static class PolymorphicContext
    {
        public static List<JsonDerivedType> DerivedTypes { get; set; } = new();
    }
}