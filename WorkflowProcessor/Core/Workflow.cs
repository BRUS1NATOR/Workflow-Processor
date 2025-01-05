using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core
{
    public class Workflow : IWorkflowInfo
    {
        public string Name { get; set; }
        public int Version { get; set; }
        public Context ContextData { get; set; }
        public WorkflowScheme Scheme { get; set; } = new();

        public Workflow()
        {

        }

        public Workflow(string name, int version)
        {
            Name = name;
            Version = version;
        }
    }
}