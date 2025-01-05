using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Core.WorkflowElement;

namespace WorkflowProcessor.Activities.Basic
{
    public interface IBlockingActivity : IWorkflowElement
    {
    }

    public class BlockingActivity : WorkflowElement, IBlockingActivity
    {
        public override async Task<WorkflowResult> ExecuteAsync(IWorkflowInstance context)
        {
            return await Task.FromResult(WorkflowBlockingActivityResult.Bookmark(context));
        }
    }
}