using WorkflowProcessor.Core.Results.Interfaces;

namespace WorkflowProcessor.Core.Results
{
    public class WorkflowExecutionResultEnd : IWorkflowResult
    {
        public int WorkflowInstanceId { get; set; }
        public string? NextStepId { get; set; }

        public static WorkflowResult Next(IWorkflowInstance workflowInstance)
        {
            return new WorkflowResult
            {
                WorkflowInstanceId = workflowInstance.Id
            };
        }
    }
}