namespace WorkflowProcessor.Core.ExecutionResults.Interfaces
{
    public interface IActivityExecutionResult : IResult
    {
        long WorkflowInstanceId { get; set; }
    }
}