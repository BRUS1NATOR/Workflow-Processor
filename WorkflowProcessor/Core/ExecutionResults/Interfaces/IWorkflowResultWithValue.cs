namespace WorkflowProcessor.Core.Results.Interfaces
{
    public interface IWorkflowResultWithValue : IWorkflowResult
    {
        object? Value { get; }
    }
}