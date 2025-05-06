using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities.Gateways
{
    public class If : ExclusiveGateway<bool>
    {

    }

    public class If<TContextData> : ExclusiveGateway<TContextData, bool>
        where TContextData : IContextData, new()
    {

    }
}