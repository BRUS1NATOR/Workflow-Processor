using Microsoft.Extensions.DependencyInjection;
using WorkflowProcessor.Core;
using WorkflowProcessor.MasstransitWorkflow;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterContext(this IServiceCollection services)
        {
        }

        public static void AddWorkflowServices(this IServiceCollection services)
        {
            services.AddSingleton<WorkflowStorage>();
            services.AddTransient<WorkflowBookmarkService>();
            //
            services.AddTransient<WorkflowExecutor>();
            services.AddTransient<WorkflowSender>();
        }
    }
}
