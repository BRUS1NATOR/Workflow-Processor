using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core.WorkflowElement
{
    public class WorkflowElement : IWorkflowElement
    {
        public WorkflowElementMetadata Metadata { get; set; }
        public void SetMetadata(WorkflowElementMetadata metadata)
        {
            Metadata = metadata;
        }

        public virtual async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance instance)
        {
            await Task.FromResult(0);
            return WorkflowResult.Next(instance);
        }
    }

    public class WorkflowElement<TContext> : WorkflowElement, IWorkflowElement<TContext> where TContext : IContextData, new()
    {
        public TContext Data(IWorkflowInstance instance)
        {
            return (TContext)instance.Context.Data;
        }
    }
}