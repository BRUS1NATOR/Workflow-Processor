using WorkflowProcessor.Bus.Models;

namespace WorkflowProcessor.Bus
{
    public interface IWorkflowMessageProducer
    {
        Task SendMessageAsync(IWorkflowMessage message);
        Task SendExecuteNextAsync(WorkflowExecuteStepMessage message);
        Task SendFinishAsync(WorkflowInstanceFinishMessage message);
        Task SendStartAsync(WorkflowStartMessage message);
    }
}