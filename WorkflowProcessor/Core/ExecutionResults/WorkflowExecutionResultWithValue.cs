using WorkflowProcessor.Core.Results.Interfaces;

namespace WorkflowProcessor.Core.Results
{
    public class WorkflowExecutionResultWithValue : WorkflowResult, IWorkflowResultWithValue
    {
        public object? Value { get; set; }

        public static WorkflowExecutionResultWithValue Next(IWorkflowInstance workflowInstance, object? value)
        {
            return new WorkflowExecutionResultWithValue
            {
                WorkflowInstanceId = workflowInstance.Id,
                Value = value
            };
        }
    }
}