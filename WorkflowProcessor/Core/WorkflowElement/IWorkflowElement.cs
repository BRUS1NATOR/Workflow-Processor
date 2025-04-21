using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core.WorkflowElement
{
    public interface IWorkflowElement
    {
        public Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance);
    }

    public interface IWorkflowElement<TContextData> : IWorkflowElement
        where TContextData : IContextData, new()
    {
        public Context<TContextData> Data(IWorkflowInstance instance);
    }
}