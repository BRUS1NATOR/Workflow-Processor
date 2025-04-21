using MassTransit;
using WorkflowProcessor.MasstransitWorkflow;

namespace WorkflowProcessor.Extensions
{
    public static class BusExtension
    {
        public static void AddWorkflowConsumers(this IBusRegistrationConfigurator bus)
        {
            bus.AddConsumer<WorkflowStartConsumer>();
            bus.AddConsumer<WorkflowFinishConsumer>();
            bus.AddConsumer<WorkflowExecuteStepConsumer>();
        }
    }
}
