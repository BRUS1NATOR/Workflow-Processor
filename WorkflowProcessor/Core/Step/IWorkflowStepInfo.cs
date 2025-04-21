using System.Text.Json.Serialization;

namespace WorkflowProcessor.Core.Step
{
    public interface IWorkflowStepInfo
    {
        [JsonPropertyName("activityId")]
        public string StepId { get; }

        [JsonPropertyName("activityType")]
        public string ActivityTypeName { get; }
    }
}
