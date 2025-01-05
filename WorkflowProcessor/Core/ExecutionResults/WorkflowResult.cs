using WorkflowProcessor.Core.Results.Interfaces;

namespace WorkflowProcessor.Core.Results
{
    public class WorkflowResult : IWorkflowResult
    {
        public int WorkflowInstanceId { get; set; }

        public static WorkflowResult Next(IWorkflowInstance workflowInstance)
        {
            return new WorkflowResult
            {
                WorkflowInstanceId = workflowInstance.Id
            };
        }
    }
}