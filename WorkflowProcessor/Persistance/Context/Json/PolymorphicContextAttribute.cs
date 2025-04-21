using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace WorkflowProcessor.Persistance.Context.Json
{
    public class PolymorphicContextAttribute : JsonDerivedTypeAttribute
    {
        public PolymorphicContextAttribute(Type derivedType, string name) : base(derivedType, name)
        {
        }

        public JsonDerivedType GetJsonDerivedType() {
            return new JsonDerivedType(DerivedType, TypeDiscriminator!.ToString()!);
        }
    }
}
