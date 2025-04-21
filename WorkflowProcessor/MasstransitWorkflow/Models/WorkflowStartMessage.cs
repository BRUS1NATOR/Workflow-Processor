namespace WorkflowProcessor.MasstransitWorkflow.Models
{
    public class WorkflowStartMessage
    {
        public string WorkflowName { get; set; }
        public int? Version { get; set; }
    }
}
