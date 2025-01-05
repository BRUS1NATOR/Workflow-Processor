namespace WorkflowProcessor.Core.Connections
{
    public interface IConnection
    {
        WorkflowStep Source { get; set; }
        WorkflowStep Target { get; set; }
    }
}