using System.Text.Json;

namespace WorkflowProcessor.Persistance.Context
{
    public class Context
    {
        public virtual object? Data { get; set; }
        public virtual void SetContextValueFromJson(string? jsonData)
        {
            if (jsonData is null)
            {
                Data = new();
                return;
            }
            JsonSerializerOptions options = new() { AllowOutOfOrderMetadataProperties = true };
            options.TypeInfoResolver = new PolymorphicTypeResolver();
            Data = JsonSerializer.Deserialize<IContextData>(jsonData, options);
        }

        public virtual string GetJsonContextValue()
        {
            JsonSerializerOptions options = new() { AllowOutOfOrderMetadataProperties = true };
            options.TypeInfoResolver = new PolymorphicTypeResolver();
            return JsonSerializer.Serialize(Data, options);
        }
    }

    public class Context<T> : Context where T : IContextData, new()
    {
        public override object? Data { get => GenericData; set => GenericData = value == null ? default : (T)value; }

        public T? GenericData { get; set; } = new();


        public override void SetContextValueFromJson(string? jsonData)
        {
            if (jsonData is null)
            {
                GenericData = new T();
                return;
            }
            GenericData = JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}