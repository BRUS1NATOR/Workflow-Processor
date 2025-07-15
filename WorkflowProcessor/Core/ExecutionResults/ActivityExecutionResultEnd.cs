using WorkflowProcessor.Core.ExecutionResults.Interfaces;

namespace WorkflowProcessor.Core.ExecutionResults
{
    public class ActivityExecutionResultEnd : IActivityExecutionResult
    {
        public long WorkflowInstanceId { get; set; }
        public ExecutionResultStatusCode StatusCode { get; } = ExecutionResultStatusCode.Finish;
        public bool IsSuccess => StatusCode != ExecutionResultStatusCode.Error;
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