using System.ComponentModel.DataAnnotations.Schema;
using WorkflowProcessor.Core.Enums;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core
{
    public interface IWorkflowInstance
    {
        public int Id { get; set; }
        public WorkflowInstanceStatus Status { get; set; }

        public Workflow Workflow { get; set; }

        public List<WorkflowExecutionPoint> WorkflowExecutionPoints { get; set; }

        public string JsonData { get; set; }
        public Context Context { get; set; }
    }

    public class WorkflowInstance : IWorkflowInstance
    {
        public int Id { get; set; }
        public WorkflowInstanceStatus Status { get; set; }

        public Workflow Workflow { get; set; }

        public List<WorkflowExecutionPoint> WorkflowExecutionPoints { get; set; } = new();

        // Данные контекста в БД
        public string? JsonData { get; set; }
        public Context Context { get; set; }
    }
}