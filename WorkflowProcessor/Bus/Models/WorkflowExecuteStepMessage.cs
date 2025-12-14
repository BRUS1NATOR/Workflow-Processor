namespace WorkflowProcessor.Bus.Models
{
    public class WorkflowExecuteStepMessage : IWorkflowMessage
    {
        public long WorkflowInstanceId { get; set; }
        public string StepId { get; set; }
        public long? PreviousExecutionPointId { get; set; }
        public WorkflowMessageType WorkflowMessageType => WorkflowMessageType.EXECUTE_NEXT_STEP;

        public WorkflowExecuteStepMessage()
        {

        }

        public WorkflowExecuteStepMessage(long workflowInstanceId, string nextStepId, long? previousExecutionPointId = null)
        {
            WorkflowInstanceId = workflowInstanceId;
            StepId = nextStepId;
            PreviousExecutionPointId = previousExecutionPointId;
        }
    }
}
