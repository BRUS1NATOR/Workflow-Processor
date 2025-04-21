// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Enums;
using WorkflowProcessor.Core.Step;

public class WorkflowExecutionPoint : IWorkflowStepInfo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    //
    [JsonPropertyName("stepId")]
    public string StepId { get; set; }
    //
    [JsonPropertyName("activatedStepsId")]
    public string[] ActivatedStepsId { get; set; } = [];

    [JsonPropertyName("activityType")]
    public string ActivityTypeName { get; set; }
    //

    [JsonPropertyName("status")]
    public WorkflowExecutionStepStatus Status { get; set; }

    [JsonPropertyName("workflowInstance")]
    public WorkflowInstance WorkflowInstance { get; set; }

    [JsonPropertyName("workflowInstanceId")]
    public long WorkflowInstanceId { get; set; }

    /// <summary>
    /// Закладка
    /// </summary>
    [JsonIgnore]
    public WorkflowBookmark? WorkflowBookmark { get; set; }
}