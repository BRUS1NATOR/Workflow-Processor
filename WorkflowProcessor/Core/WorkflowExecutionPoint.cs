// See https://aka.ms/new-console-template for more information
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Enums;

public class WorkflowExecutionPoint
{
    public int Id { get; set; }
    public string StepId { get; set; }
    public string ActivityType { get; set; }

    public WorkflowExecutionStepStatus Status { get; set; }

    public WorkflowInstance WorkflowInstance { get; set; }
    public int WorkflowInstanceId { get; set; }

    /// <summary>
    /// Закладка
    /// </summary>
    public WorkflowBookmark? WorkflowBookmark { get; set; }
}