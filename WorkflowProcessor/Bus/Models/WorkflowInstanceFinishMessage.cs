namespace WorkflowProcessor.Bus.Models
{
    public class WorkflowInstanceFinishMessage : IWorkflowMessage
    {
        public long WorkflowInstanceId { get; set; }
        public WorkflowMessageType WorkflowMessageType => WorkflowMessageType.FINISH;
    }
}
