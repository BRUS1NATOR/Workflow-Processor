using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Core.WorkflowElement;

namespace WorkflowProcessor.Activities.Gateways
{
    [ActivityType(BaseAcitivityType.ParallelGatewayEnd)]
    public class ParallelExclusiveGatewayEnd : WorkflowElement, IParallelExclusiveGatewayEnd
    {
        public string GatewayStartStepId { get; set; }
    }

    [ActivityType(BaseAcitivityType.ParallelGatewayEnd)]
    public interface IParallelExclusiveGatewayEnd
    {
        public string GatewayStartStepId { get;set; }
    }
}
