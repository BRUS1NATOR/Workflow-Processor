using WorkflowProcessor.Core.Results.Interfaces;

namespace WorkflowProcessor.Core.Results
{
    public class WorkflowBlockingActivityResult : WorkflowResult, IWorkflowBlockingResult
    {
        public bool CreateBookmark { get; set; }

        public static WorkflowBlockingActivityResult Bookmark(IWorkflowInstance workflowInstance, bool createBookMark = true)
        {
            return new WorkflowBlockingActivityResult
            {
                CreateBookmark = createBookMark,
                WorkflowInstanceId = workflowInstance.Id
            };
        }
    }
}