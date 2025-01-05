using MassTransitExample.Examples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Extensions
{
    public static class AppExtension
    {
        public static void AddWorkflows(this IHost host)
        {
            // Запустить процесс
            var _workflowStorage = host.Services.GetRequiredService<WorkflowStorage>();
            _workflowStorage.AddWorkflow<TestProcess1>();
            _workflowStorage.AddWorkflow<TestProcess2>();
            _workflowStorage.AddWorkflow<TestProcess3>();
            _workflowStorage.AddWorkflow<TestProcess4>();
            _workflowStorage.AddWorkflow<ApprovementExample>();
        }
    }
}
