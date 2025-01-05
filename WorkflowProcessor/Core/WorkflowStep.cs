using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core
{
    public class WorkflowStep
    {
        public string Id { get; set; }
        public string ActivityId { get; set; }
        public virtual string ActivityType => "Activity";

        public WorkflowStep()
        {
        }

        public virtual Type GetWorkflowElementType()
        {
            return typeof(IWorkflowElement);
        }

        public virtual void SetupWorkflowInstance(object instance, object context)
        {

        }
    }

    public class WorkflowStep<TWorkflowElement> : WorkflowStep where TWorkflowElement : IWorkflowElement
    {
        public Action<TWorkflowElement>? Action { get; set; }
        public override string ActivityType { get { return typeof(TWorkflowElement).Name; } }

        public WorkflowStep() : base()
        {
        }

        public WorkflowStep(Action<TWorkflowElement> action)
        {
            Action = action;
        }

        public override Type GetWorkflowElementType()
        {
            return typeof(TWorkflowElement);
        }

        public override void SetupWorkflowInstance(object instance, object context)
        {
            if (Action is null)
            {
                return;
            }
            Action((TWorkflowElement)instance);
        }

        public virtual WorkflowStep<TWorkflowElement> WithId(string activityId)
        {
            ActivityId = activityId;
            return this;
        }
    }

    public class WorkflowStep<TWorkflowElement, TContext> : WorkflowStep where TContext : IContextData, new() where TWorkflowElement : IWorkflowElement<TContext>
    {
        public Action<TWorkflowElement, TContext>? Action { get; set; }
        public override string ActivityType { get { return typeof(TWorkflowElement).Name; } }

        public WorkflowStep() : base()
        {
        }

        public WorkflowStep(Action<TWorkflowElement, TContext> action)
        {
            Action = action;
        }

        public override Type GetWorkflowElementType()
        {
            return typeof(TWorkflowElement);
        }
    
        public override void SetupWorkflowInstance(object instance, object context)
        {
            if (Action is null)
            {
                return;
            }
            Action((TWorkflowElement)instance, (TContext)context);
        }

        public WorkflowStep<TWorkflowElement, TContext> WithId(string activityId)
        {
            ActivityId = activityId;
            return this;
        }
    }
}
