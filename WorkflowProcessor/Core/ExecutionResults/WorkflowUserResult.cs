using WorkflowProcessor.Core.Results.Interfaces;

namespace WorkflowProcessor.Core.Results
{
    public class WorkflowUserResult : WorkflowBlockingActivityResult, IWorkflowResultWithValue, IWorkflowUserResult
    {
        public object? Value => NextStepId;
        public string NextStepId { get; set; }
        public int UserId { get; set; }
        public string TaskName { get; set; }

        public static WorkflowUserResult UserTask(IWorkflowInstance workflowInstance, int UserId, string TaskName)
        {
            return new WorkflowUserResult
            {
                CreateBookmark = true,
                WorkflowInstanceId = workflowInstance.Id,
                UserId = UserId,
                TaskName = TaskName
            };
        }
    }

    public interface IWorkflowUserResult : IWorkflowBlockingResult
    { 
        public int UserId { get; set; }
        public string TaskName { get; set; }
    }
}