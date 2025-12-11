using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public interface IWorkflowFinishConsumer
    {
        Task ConsumeAsync(WorkflowInstanceFinishMessage message);
    }
}