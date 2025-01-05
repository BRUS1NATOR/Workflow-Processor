namespace WorkflowProcessor.Core.Results.Interfaces
{
    public interface IWorkflowBlockingResult : IWorkflowResult
    {
        bool CreateBookmark { get; set; }
    }
}