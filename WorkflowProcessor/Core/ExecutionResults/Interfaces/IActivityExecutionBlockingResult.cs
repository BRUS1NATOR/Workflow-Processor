namespace WorkflowProcessor.Core.ExecutionResults.Interfaces
{
    /// <summary>
    /// Result that blocks workflow instance flow and creates bookmark
    /// </summary>
    public interface IActivityExecutionBlockingResult : IActivityExecutionResult
    {
        long? ChildWorkflowInstanceId { get; set; }
        string? BookmarkName { get; set; }
    }
}