using Microsoft.Extensions.DependencyInjection;
using WorkflowProcessor.Core;
using WorkflowProcessor.MasstransitWorkflow;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Extensions
{
    public static class ServiceExtension
    {
        public static void AddWorkflowServices(this IServiceCollection services)
        {
            services.AddSingleton<WorkflowStorage>();
            services.AddTransient<WorkflowBookmarkService>();
            //
            services.AddTransient<WorkflowExecutor>();
        }

        public static void AddWorkflowInMemoryConsumers(this IServiceCollection services)
        {
            services.AddTransient<WorkflowStartConsumer>();
            services.AddTransient<WorkflowFinishConsumer>();
            services.AddTransient<WorkflowExecuteStepConsumer>();
            services.AddTransient<IWorkflowMessageProducer, WorkflowMessageInMemoryProducer>();
        }
    }
}
