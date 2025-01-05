using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities.Basic
{
    public class CodeActivity<TContext> : WorkflowElement<TContext> where TContext : IContextData, new()
    {
        protected Action<TContext> ExecuteCode;

        public void Code(Action<TContext> ExecuteCode)
        {
            this.ExecuteCode = ExecuteCode;
        }

        public override async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance workflowInstance)
        {
            var data = Data(workflowInstance);
            ExecuteCode.Invoke(data);
            return await base.ExecuteAsync(workflowInstance);
        }
    }
}