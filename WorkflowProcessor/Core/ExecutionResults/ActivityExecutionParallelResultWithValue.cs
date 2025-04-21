using WorkflowProcessor.Core.ExecutionResults.Interfaces;

namespace WorkflowProcessor.Core.ExecutionResults
{
    public class ActivityExecutionParallelResultWithValue : ActivityExecutionResultWithValue, IActivityExecutionParallelResult
    {

        public static ActivityExecutionParallelResultWithValue Next(IWorkflowInstance workflowInstance, object? value)
        {
            return new ActivityExecutionParallelResultWithValue
            {
                WorkflowInstanceId = workflowInstance.Id,
                Value = value
            };
        }
    }
}