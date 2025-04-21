using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.WorkflowElement;

namespace WorkflowProcessor.Activities.Basic
{
    public class EndActivity : WorkflowElement
    {
        public EndActivity()
        {
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance instance)
        {
            return await Task.FromResult(ActivityExecutionResult.Next(instance));
        }
    }
}
