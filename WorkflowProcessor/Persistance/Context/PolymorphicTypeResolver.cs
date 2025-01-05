using MassTransitExample.Examples;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace WorkflowProcessor.Persistance.Context
{
    public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            Type basePointType = typeof(IContextData);
            if (jsonTypeInfo.Type == basePointType)
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "$ContextType",
                    IgnoreUnrecognizedTypeDiscriminators = true,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor,

                    DerivedTypes =
                    {
                        new JsonDerivedType(typeof(Data1), "Data1"),
                        new JsonDerivedType(typeof(Data2), "Data2"),
                        new JsonDerivedType(typeof(Data3), "Data3"),
                        new JsonDerivedType(typeof(ApprovementData), "ApprovementData")
                    }
                };
            }

            return jsonTypeInfo;
        }
    }
}