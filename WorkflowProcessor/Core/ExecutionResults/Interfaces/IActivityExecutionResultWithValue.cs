namespace WorkflowProcessor.Core.ExecutionResults.Interfaces
{
    public interface IActivityExecutionResultWithValue : IActivityExecutionResult
    {
        object? Value { get; }
    }
}