namespace WorkflowProcessor.Bus.Models
{
    public interface IWorkflowMessage
    {
        WorkflowMessageType WorkflowMessageType { get; }
    }
}