using WorkflowProcessor.Core;

namespace WorkflowProcessor.Services
{
    public interface IWorkflowStorage
    {
        Workflow AddWorkflow<T>() where T : WorkflowBuilder, new();
        Workflow? GetWorkflow(IWorkflowIdentifier workflowInfo);
        Workflow? GetWorkflow(IWorkflowInstance instance);
        Workflow? GetWorkflow(string name, int? version);
    }
}