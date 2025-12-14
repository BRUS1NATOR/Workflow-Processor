namespace WorkflowProcessor.Bus.Models
{
    public class WorkflowStartMessage : IWorkflowMessage
    {
        public string WorkflowName { get; set; }
        public int? Version { get; set; }

        public WorkflowMessageType WorkflowMessageType => WorkflowMessageType.START;
    }
}
