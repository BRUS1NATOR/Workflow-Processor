using WorkflowProcessor.Core.ExecutionResults.Interfaces;

namespace WorkflowProcessor.Core.ExecutionResults
{
    public enum ExecutionResultStatusCode
    {
        Next = 1,
        Finish = 2,
        Error = 3
    }

    /// <summary>
    /// Activity result
    /// </summary>
    public class ActivityExecutionResult : IActivityExecutionResult
    {
        public long WorkflowInstanceId { get; set; }
        public ExecutionResultStatusCode StatusCode { get; set; } = ExecutionResultStatusCode.Next;
        public bool IsSuccess => StatusCode != ExecutionResultStatusCode.Error;
        public string Message { get; set;  }

        public static ActivityExecutionResult Error(IWorkflowInstance workflowInstance)
        {
            return new ActivityExecutionResult
            {
                StatusCode = ExecutionResultStatusCode.Error,
                WorkflowInstanceId = workflowInstance.Id
            };
        }

        public static ActivityExecutionResult Next(IWorkflowInstance workflowInstance)
        {
            return new ActivityExecutionResult
            {
                StatusCode = ExecutionResultStatusCode.Next,
                WorkflowInstanceId = workflowInstance.Id
            };
        }

        public static ActivityExecutionResult Finish(IWorkflowInstance workflowInstance)
        {
            return new ActivityExecutionResult
            {
                StatusCode = ExecutionResultStatusCode.Finish,
                WorkflowInstanceId = workflowInstance.Id
            };
        }
    }
}