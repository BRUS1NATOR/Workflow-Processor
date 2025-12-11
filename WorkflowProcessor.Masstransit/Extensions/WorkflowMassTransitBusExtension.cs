using MassTransit;
using WorkflowProcessor.MasstransitWorkflow;

namespace WorkflowProcessor.MassTransit.Extensions
{
    public static class WorkflowMassTransitBusExtension
    {
        public static void AddWorkflowMassTransitConsumers(this IBusRegistrationConfigurator bus)
        {
            bus.AddConsumer<WorkflowStartConsumerMassTransit>();
            bus.AddConsumer<WorkflowFinishConsumerMassTransit>();
            bus.AddConsumer<WorkflowExecuteStepConsumerMassTransit>();
        }
    }
}
