using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities
{

    public class If : Gateway<bool>
    {

    }

    public class If<TContext> : Gateway<TContext, bool> where TContext : IContextData, new()
    {

    }
}