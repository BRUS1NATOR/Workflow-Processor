using WorkflowProcessor.Core.ExecutionResults.Interfaces;
using WorkflowProcessor.Core.ExecutionResults.Interfaces;

namespace WorkflowProcessor.Core.ExecutionResults
{
    /// <summary>
    /// Result that blocks workflow instance flow and creates user task (bookmark)
    /// </summary>
    public class ActivityExecutionUserResult : ActivityExecutionBlockingResult, IActivityExecutionResultWithValue, IActivityExecutionUserResult
    {
        public object? Value => NextStepId;
        public string NextStepId { get; set; }
        public List<long> Users { get; set; }

        public static ActivityExecutionUserResult UserTask(IWorkflowInstance workflowInstance, List<long> users, string taskName)
        {
            return new ActivityExecutionUserResult
            {
                WorkflowInstanceId = workflowInstance.Id,
                Users = users,
                BookmarkName = taskName
            };
        }
    }
}