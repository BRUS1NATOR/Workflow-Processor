using System.Reflection;
using WorkflowProcessor.Core;
using WorkflowProcessor.Persistance.Context.Json;

namespace WorkflowProcessor.Services
{
    public class WorkflowStorage
    {
        public List<Workflow> Workflows = new();

        public void AddWorkflow<T>() where T : WorkflowBuilder, new()
        {
            if (typeof(T).BaseType.IsGenericType)
            {
                foreach (var generic in typeof(T).BaseType.GetGenericArguments())
                {
                    var polymorphicAttribute = generic.GetCustomAttribute<PolymorphicContextAttribute>();
                    if (polymorphicAttribute is not null)
                    {
                        PolymorphicContext.DerivedTypes.Add(polymorphicAttribute.GetJsonDerivedType());
                    }
                }
            }
            Workflows.Add(new T().Build());
        }


        public Workflow? GetWorkflow(IWorkflowIdentifier workflowInfo)
        {
            return GetWorkflow(workflowInfo.Name, workflowInfo.Version);
        }

        public Workflow? GetWorkflow(IWorkflowInstance instance)
        {
            return GetWorkflow(instance.WorkflowInfo);
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
