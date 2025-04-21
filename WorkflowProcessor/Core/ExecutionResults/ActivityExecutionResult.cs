using WorkflowProcessor.Core.ExecutionResults.Interfaces;

namespace WorkflowProcessor.Core.ExecutionResults
{
    /// <summary>
    /// Activity result
    /// </summary>
    public class ActivityExecutionResult : IActivityExecutionResult
    {
        public long WorkflowInstanceId { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set;  }

        public static ActivityExecutionResult Error(IWorkflowInstance workflowInstance)
        {
            return new ActivityExecutionResult
            {
                IsSuccess = false,
                WorkflowInstanceId = workflowInstance.Id
            };
        }

        public static ActivityExecutionResult Next(IWorkflowInstance workflowInstance)
        {
            return new ActivityExecutionResult
            {
                WorkflowInstanceId = workflowInstance.Id
            };
        }
    }
}