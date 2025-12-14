using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Extensions
{
    public static class AppExtension
    {
        public static void AddWorkflow<T>(this IHost host) where T : WorkflowBuilder, new()
        {
            var _workflowStorage = host.Services.GetRequiredService<IWorkflowStorage>();
            _workflowStorage.AddWorkflow<T>();
        }
    }
}
