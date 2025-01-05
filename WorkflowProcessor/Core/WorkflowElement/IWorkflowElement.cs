using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core.WorkflowElement
{
    public interface IWorkflowElement
    {
        public void SetMetadata(WorkflowElementMetadata metadata);
        public WorkflowElementMetadata? Metadata { get; }

        public Task<WorkflowResult> ExecuteAsync(IWorkflowInstance instance);
    }

    public interface IWorkflowElement<TContext> : IWorkflowElement where TContext : IContextData, new()
    {
        public TContext Data(IWorkflowInstance instance);
    }
}