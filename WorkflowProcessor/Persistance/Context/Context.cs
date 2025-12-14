using System.Text.Json;
using System.Text.Json.Serialization;
using WorkflowProcessor.Core;
using WorkflowProcessor.Persistance.Context.Json;

namespace WorkflowProcessor.Persistance.Context
{
    public class Context
    {
        [JsonIgnore]
        public string JsonData { get; set; }

        [JsonPropertyName("data")]
        public virtual object? DataObject { get; set; }

        [JsonIgnore]
        public WorkflowInstance WorkflowInstance { get; set; }

        public Context(string jsonData)
        {
            this.JsonData = jsonData;
            if (!string.IsNullOrEmpty(jsonData))
            {
                SetContextDataFromJson(JsonData);
            }
        }

        public Context(object data)
        {
            DataObject = data;
        }

        public virtual void SetContextDataFromJson(string? jsonData)
        {
            if (jsonData is null)
            {
                DataObject = new();
                return;
            }
            JsonSerializerOptions options = new() { /*AllowOutOfOrderMetadataProperties = true*/ };
            options.TypeInfoResolver = new PolymorphicContextResolver();
            try
            {
                DataObject = JsonSerializer.Deserialize<IContextData>(jsonData, options);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Сохраняет DataObject в Json
        /// </summary>
        /// <returns></returns>
        public virtual void Save()
        {
            JsonData = JsonSerializer.Serialize(DataObject);
        }
    }

    public class Context<T> : Context where T : IContextData, new()
    {
        [JsonPropertyName("data")]
        public override object? DataObject { get => Data; set => Data = value == null ? default : (T)value; }

        [JsonIgnore]
        public T Data { get; set; } = new();

        public Context(T data) : base("")
        {
            Data = data;
        }

        public Context(string jsonData) : base(jsonData)
        {

        }


        public virtual void SetContextData(T data)
        {
            Data = data;
        }

        public override void SetContextDataFromJson(string? jsonData)
        {
            if (jsonData is null)
            {
                Data = new T();
                return;
            }
            Data = JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}