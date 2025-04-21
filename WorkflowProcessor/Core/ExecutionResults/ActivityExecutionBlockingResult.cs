using WorkflowProcessor.Core.ExecutionResults.Interfaces;

namespace WorkflowProcessor.Core.ExecutionResults
{
    /// <summary>
    /// Result that blocks workflow instance flow and creates user task (bookmark)
    /// </summary>
    public class ActivityExecutionBlockingResult : ActivityExecutionResult, IActivityExecutionBlockingResult
    {
        public long? ChildWorkflowInstanceId { get; set; }
        public string? BookmarkName { get; set; }

        public static ActivityExecutionBlockingResult Bookmark(IWorkflowInstance workflowInstance, string bookMarkName = "", long? childWorkflowInstance = null)
        {
            return new ActivityExecutionBlockingResult
            {
                WorkflowInstanceId = workflowInstance.Id,
                BookmarkName = bookMarkName,
                ChildWorkflowInstanceId = childWorkflowInstance
            };
        }
    }
}