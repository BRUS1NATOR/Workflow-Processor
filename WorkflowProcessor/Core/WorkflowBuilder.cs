using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core
{
    public abstract class WorkflowBuilder : IWorkflowInfo
    {
        public string Name { get; set; }
        public int Version { get; set; }

        protected List<WorkflowStep> Elements = new();
        protected List<Connection> Connections = new();

        public WorkflowBuilder()
        {

        }

        public WorkflowBuilder(string name, int version)
        {
            Name = name;
            Version = version;
        }

        public WorkflowStep<TElement> Step<TElement>() where TElement : IWorkflowElement
        {
            WorkflowStep<TElement> workflowStep = new WorkflowStep<TElement>();
            Elements.Add(workflowStep);
            return workflowStep;
        }

        public WorkflowStep<TElement> Step<TElement>(Action<TElement> action) where TElement : IWorkflowElement
        {
            WorkflowStep<TElement> workflowStep = new WorkflowStep<TElement>(action);
            Elements.Add(workflowStep);
            return workflowStep;
        }

        protected WorkflowScheme BuildScheme()
        {
            return new WorkflowScheme(Elements, Connections).Init();
        }

        public virtual Workflow Build()
        {
            return new Workflow()
            {
                Scheme = BuildScheme(),
                ContextData = new Context(),
                Name = Name,
                Version = Version
            };
        }
    }

    public abstract class WorkflowBuilder<TContext> : WorkflowBuilder where TContext : IContextData, new()
    {
        public Context<TContext> GenericContextData { get; set; } = new();

        public WorkflowStep<TElement, TContext> Step<TElement>(Action<TElement, TContext> action) where TElement : IWorkflowElement<TContext>
        {
            WorkflowStep<TElement, TContext> workflowStep = new WorkflowStep<TElement, TContext>(action);
            Elements.Add(workflowStep);
            return workflowStep;
        }

        public override Workflow Build()
        {
            return new Workflow()
            {
                Scheme = BuildScheme(),
                ContextData = new Context<TContext>(),
                Name = Name,
                Version = Version
            };
        }
    }
}