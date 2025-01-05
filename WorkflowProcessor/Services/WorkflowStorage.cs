using WorkflowProcessor.Core;

namespace WorkflowProcessor.Services
{
    public class WorkflowStorage
    {
        public List<Workflow> Workflows = new();

        public void AddWorkflow<T>() where T : WorkflowBuilder, new()
        {
            Workflows.Add(new T().Build());
        }

        public IEnumerable<Workflow> GetWorkflowList()
        {
            return Workflows;
        }

        public Workflow? GetWorkflow(IWorkflowInfo workflowInfo)
        {
            return GetWorkflow(workflowInfo.Name, workflowInfo.Version);
        }

        public Workflow? GetWorkflow(IWorkflowInstance instance)
        {
            return GetWorkflow(instance.Workflow);
        }

        public Workflow? GetWorkflow(string name, int? version)
        {
            if (version is null)
            {
                return Workflows.Where(x => x.Name == name).OrderByDescending(x => x.Version).FirstOrDefault()!;
            }
            return Workflows.Where(x => x.Name == name && x.Version == version).FirstOrDefault()!;
        }
    }
}
