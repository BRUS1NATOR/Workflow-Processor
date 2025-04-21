using WorkflowProcessor.Core.ExecutionResults.Interfaces;

namespace WorkflowProcessor.Core.ExecutionResults
{
    public class ActivityExecutionResultWithValue : ActivityExecutionResult, IActivityExecutionResultWithValue
    {
        public object? Value { get; set; }

        public static ActivityExecutionResultWithValue Next(IWorkflowInstance workflowInstance, object? value)
        {
            return new ActivityExecutionResultWithValue
            {
                WorkflowInstanceId = workflowInstance.Id,
                Value = value
            };
        }
    }
}