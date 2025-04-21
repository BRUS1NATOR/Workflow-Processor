using System.Text.Json.Serialization;
using WorkflowProcessor.Core.Enums;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core
{
    public class WorkflowInstance : IWorkflowInstance
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }

        [JsonIgnore]
        public WorkflowInstance? Parent { get; set; }

        [JsonIgnore]
        public List<WorkflowInstance> Children { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("status")]
        public WorkflowInstanceStatus Status { get; set; }

        [JsonPropertyName("initiator")]
        public long? Initiator { get; set; }
        /// <summary>
        /// Данные контекста
        /// </summary>
        [JsonPropertyName("context")]
        public Context Context { get; set; }

        [JsonIgnore]
        public List<WorkflowExecutionPoint> WorkflowExecutionPoints { get; set; } = new();

        [JsonPropertyName("workflowInfo")]
        public WorkflowInfo WorkflowInfo { get; set; }

        public Context<T> AsContext<T>() where T : IContextData, new()
        {
            var contextGeneric = new Context<T>(Context.JsonData);
            Context = contextGeneric;
            Context.WorkflowInstance = this;
            return contextGeneric;
        }
    }
}