using System.Text.Json.Serialization;

namespace WorkflowProcessor.Core.Connections.Metadata
{
    public class ConnectionMetadata : IConnectionMetadata
    {
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("connectionKey")]
        public string? ConnectionKey { get; set; }

        public ConnectionMetadata()
        {
            
        }

        public ConnectionMetadata(string? displayName)
        {
            DisplayName = displayName;
        }
        public ConnectionMetadata(string? displayName, string? connectionKey)
        {
            DisplayName = displayName;
            ConnectionKey = connectionKey;
        }
    }
}
