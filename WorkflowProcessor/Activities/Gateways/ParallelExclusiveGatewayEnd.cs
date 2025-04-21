using WorkflowProcessor.Core.WorkflowElement;

namespace WorkflowProcessor.Activities.Gateways
{
    public class ParallelExclusiveGatewayEnd : WorkflowElement, IParallelExclusiveGatewayEnd
    {
        public string GatewayStartStepId { get; set; }
    }

    public interface IParallelExclusiveGatewayEnd
    {
        public string GatewayStartStepId { get;set; }
    }
}
