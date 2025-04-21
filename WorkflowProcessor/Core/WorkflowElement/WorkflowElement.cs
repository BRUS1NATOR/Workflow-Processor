using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core.WorkflowElement
{
    public class WorkflowElement : IWorkflowElement
    {
        public virtual async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            await Task.FromResult(0);
            return ActivityExecutionResult.Next(instance);
        }
    }

    public class WorkflowElement<TContextData> : WorkflowElement, IWorkflowElement<TContextData>
        where TContextData : IContextData, new()
    {
        public Context<TContextData> Data(IWorkflowInstance workflowInstance)
        {
            return workflowInstance.AsContext<TContextData>();
        }
    }
}