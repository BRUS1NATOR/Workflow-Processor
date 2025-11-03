using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkflowProcessor.Console.Examples;
using WorkflowProcessor.Core;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Extensions
{
    public static class AppExtension
    {
        public static void AddWorkflow<T>(this IHost host) where T : WorkflowBuilder, new()
        {
            var _workflowStorage = host.Services.GetRequiredService<WorkflowStorage>();
            _workflowStorage.AddWorkflow<T>();
        }

        public static void AddExampleWorkflows(this IHost host)
        {
            var _workflowStorage = host.Services.GetRequiredService<WorkflowStorage>();
            _workflowStorage.AddWorkflow<TestProcess1>();
            _workflowStorage.AddWorkflow<TestProcess2>();
            _workflowStorage.AddWorkflow<TestProcess3>();
            _workflowStorage.AddWorkflow<TestProcess4>();
            _workflowStorage.AddWorkflow<ParallelGatewayExample>();
            _workflowStorage.AddWorkflow<ApprovementExample>();
        }
    }
}
