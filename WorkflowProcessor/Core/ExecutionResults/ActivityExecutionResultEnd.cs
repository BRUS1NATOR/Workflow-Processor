using WorkflowProcessor.Core.ExecutionResults.Interfaces;

namespace WorkflowProcessor.Core.ExecutionResults
{
    public class ActivityExecutionResultEnd : IActivityExecutionResult
    {
        public long WorkflowInstanceId { get; set; }

        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }

        public static ActivityExecutionResult Next(IWorkflowInstance workflowInstance)
        {
            return new ActivityExecutionResult
            {
                WorkflowInstanceId = workflowInstance.Id
            };
        }
    }
}