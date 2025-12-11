using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public interface IWorkflowStartConsumer
    {
        Task ConsumeAsync(WorkflowStartMessage context);
    }
}