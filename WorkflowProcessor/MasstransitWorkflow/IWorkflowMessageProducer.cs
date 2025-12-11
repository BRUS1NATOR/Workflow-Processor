using WorkflowProcessor.MasstransitWorkflow.Models;

namespace WorkflowProcessor.MasstransitWorkflow
{
    public interface IWorkflowMessageProducer
    {
        Task SendExecuteNext(WorkflowExecuteStep message);
        Task SendFinish(WorkflowInstanceFinishMessage message);
        Task SendStart(WorkflowStartMessage message);
    }
}