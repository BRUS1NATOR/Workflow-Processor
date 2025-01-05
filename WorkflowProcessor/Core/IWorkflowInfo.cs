namespace WorkflowProcessor.Core
{
    public interface IWorkflowInfo
    {
        public string Name { get; set; }
        public int Version { get; set; }
    }
}