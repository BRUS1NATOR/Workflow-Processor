using System.Text.Json.Serialization;

namespace WorkflowProcessor.Core
{
    public class WorkflowInfo : IWorkflowIdentifier
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("isAllowedToRunFromWeb")]
        public bool IsAllowedToRunFromWeb { get; set; }
    }

    public interface IWorkflowIdentifier
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }
    }

    public class WorkflowIdentifier : IWorkflowIdentifier
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

    }
}