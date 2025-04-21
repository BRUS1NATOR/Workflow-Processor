using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace WorkflowProcessor.Persistance.Context.Json
{

    public class PolymorphicContextResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            Type basePointType = typeof(IContextData);
            if (jsonTypeInfo.Type == basePointType)
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "$type",
                    IgnoreUnrecognizedTypeDiscriminators = true,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
                };
                foreach (var derivedType in PolymorphicContext.DerivedTypes)
                {
                    jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
                }
            }

            return jsonTypeInfo;
        }
    }
}