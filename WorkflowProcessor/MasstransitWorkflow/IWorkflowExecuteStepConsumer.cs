using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public interface IWorkflowExecuteStepConsumer
    {
        Task ConsumeAsync(WorkflowExecuteStep executeStep);
    }
}