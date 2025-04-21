using System.Text.Json.Serialization;

namespace WorkflowProcessor.Core.Connections.Metadata
{
    public interface IConnectionMetadata
    {
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("connectionKey")]
        public string? ConnectionKey { get; set; }
    }
}
