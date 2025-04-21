using System.Text.Json.Serialization;
using WorkflowProcessor.Core.Connections.Metadata;
using WorkflowProcessor.Core.Step;

namespace WorkflowProcessor.Core.Connections
{
    public interface IConnection
    {
        [JsonPropertyName("source")]
        WorkflowStep Source { get; set; }

        [JsonPropertyName("target")]
        WorkflowStep Target { get; set; }

        [JsonPropertyName("metadata")]
        IConnectionMetadata Metadata { get; set; }
    }
}