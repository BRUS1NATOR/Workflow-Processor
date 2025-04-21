using System.Text.Json.Serialization;
using WorkflowProcessor.Core.Enums;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core
{
    public interface IWorkflowInstance
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }

        [JsonPropertyName("parent")]
        public WorkflowInstance? Parent { get; set; }
        [JsonPropertyName("children")]
        public List<WorkflowInstance> Children { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public WorkflowInstanceStatus Status { get; set; }

        [JsonPropertyName("initiator")]
        public long? Initiator { get; set; }

        [JsonPropertyName("context")]
        public Context Context { get; set; }


        [JsonIgnore]
        public List<WorkflowExecutionPoint> WorkflowExecutionPoints { get; set; }

        [JsonPropertyName("workflowInfo")]
        public WorkflowInfo WorkflowInfo { get; set; }

        public Context<T> AsContext<T>() where T : IContextData, new();
    }
}