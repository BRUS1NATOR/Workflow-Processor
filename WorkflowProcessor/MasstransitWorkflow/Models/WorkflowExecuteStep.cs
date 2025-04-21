namespace WorkflowProcessor.MasstransitWorkflow.Models
{
    public class WorkflowExecuteStep
    {
        public long WorkflowInstanceId { get; set; }
        public string StepId { get; set; }
        public long? PreviousExecutionPointId { get; set; }

        public WorkflowExecuteStep()
        {

        }

        public WorkflowExecuteStep(long workflowInstanceId, string nextStepId, long? previousExecutionPointId = null)
        {
            WorkflowInstanceId = workflowInstanceId;
            StepId = nextStepId;
            PreviousExecutionPointId = previousExecutionPointId;
        }
    }
}
