using MassTransitExample;
using MassTransitExample.Services;
using Microsoft.Extensions.DependencyInjection;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Extensions
{
    public static class ServiceExtension
    {
        public static void AddWorkflow(this IServiceCollection services)
        {
            services.AddSingleton<WorkflowStorage>();
            services.AddTransient<WorkflowExecutor>();
            services.AddTransient<WorkflowSender>();
        }
    }
}
