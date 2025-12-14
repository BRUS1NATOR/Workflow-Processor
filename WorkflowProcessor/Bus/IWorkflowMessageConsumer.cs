using WorkflowProcessor.Bus.Models;

namespace WorkflowProcessor.Bus
{
    public interface IWorkflowMessageConsumer<T> where T : IWorkflowMessage
    {
        Task ConsumeAsync(T message);
    }
}