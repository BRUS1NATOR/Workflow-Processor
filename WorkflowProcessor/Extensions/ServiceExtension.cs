using Microsoft.Extensions.DependencyInjection;
using WorkflowProcessor.Core;
using WorkflowProcessor.Bus;
using WorkflowProcessor.Services;
using WorkflowProcessor.Bus.InMemory;
using System.Threading.Channels;
using WorkflowProcessor.Bus.Models;

namespace WorkflowProcessor.Extensions
{
    public static class ServiceExtension
    {
        public static void AddWorkflowServices(this IServiceCollection services)
        {
            services.AddSingleton<IWorkflowStorage, WorkflowStorage>();
            services.AddTransient<WorkflowBookmarkService>();
            //
            services.AddTransient<WorkflowExecutor>();
        }

        public static void AddWorkflowInMemoryBus(this IServiceCollection services)
        {
            services.AddSingleton(_ =>     
                Channel.CreateUnbounded<IWorkflowMessage>(new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false
                })
            );

            services.AddTransient<WorkflowStartConsumer>();
            services.AddTransient<WorkflowFinishConsumer>();
            services.AddTransient<WorkflowExecuteStepConsumer>();
            services.AddTransient<IWorkflowMessageProducer, WorkflowMessageInMemoryProducer>();
            services.AddHostedService<WorkflowMessageInMemoryConsumer>();
        }
    }
}
