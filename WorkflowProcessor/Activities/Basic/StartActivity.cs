using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Core.WorkflowElement;

namespace WorkflowProcessor.Activities.Basic
{
    public class StartActivity : WorkflowElement
    {
        public StartActivity()
        {
        }

        public override async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance instance)
        {
            return await Task.FromResult(WorkflowResult.Next(instance));
        }
    }
}
