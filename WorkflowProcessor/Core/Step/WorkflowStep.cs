using System.Text.Json.Serialization;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core.Step
{

    public abstract class WorkflowStep : IWorkflowStepInfo
    {
        [JsonPropertyName("activityId")]
        public string StepId { get; set; } = string.Empty;

        [JsonPropertyName("activityType")]
        public virtual string ActivityTypeName => "Activity";

        [JsonIgnore]
        public virtual Type ActivityType => typeof(IWorkflowElement);

        public WorkflowStepMetadata? Metadata { get; set; }

        public WorkflowStep()
        {
        }

        public void SetMetadata(WorkflowStepMetadata metadata)
        {
            Metadata = metadata;
        }

        public virtual void Setup(object element, object context)
        {

        }
    }

    public class WorkflowStep<TWorkflowElement> : WorkflowStep where TWorkflowElement : IWorkflowElement
    {
        public Action<TWorkflowElement>? Action { get; set; }

        public override string ActivityTypeName
        {
            get
            {
                return typeof(TWorkflowElement).IsGenericType ?
                    typeof(TWorkflowElement).Name.Substring(0, typeof(TWorkflowElement).Name.IndexOf('`')):
                    typeof(TWorkflowElement).Name;
            }
        }

        public override Type ActivityType => typeof(TWorkflowElement);

        public WorkflowStep() : base()
        {
        }

        public WorkflowStep(Action<TWorkflowElement> action)
        {
            Action = action;
        }

        public override void Setup(object element, object context)
        {
            if (Action is null)
            {
                return;
            }
            Action((TWorkflowElement)element);
        }

        public virtual WorkflowStep<TWorkflowElement> WithId(string activityId)
        {
            StepId = activityId;
            return this;
        }
    }


    public class WorkflowStep<TWorkflowElement, TContextData> : WorkflowStep
        where TWorkflowElement : IWorkflowElement<TContextData>
        where TContextData : IContextData, new()
    {
        public Action<TWorkflowElement, Context<TContextData>>? Action { get; set; }
        public override string ActivityTypeName { get { return typeof(TWorkflowElement).Name; } }
        public override Type ActivityType => typeof(TWorkflowElement);

        public WorkflowStep() : base()
        {
        }

        public WorkflowStep(Action<TWorkflowElement, Context<TContextData>> action)
        {
            Action = action;
        }

        public override void Setup(object element, object context)
        {
            if (Action is null)
            {
                return;
            }
            Action((TWorkflowElement)element, (Context<TContextData>)context);
        }

        public WorkflowStep<TWorkflowElement, TContextData> WithId(string activityId)
        {
            StepId = activityId;
            return this;
        }
    }
}
