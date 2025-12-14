// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.Core;

public interface IWorkflowDbContext
{
    DbSet<UserTask> UserTasks { get; set; }
    DbSet<WorkflowBookmark> WorkflowBookmarks { get; set; }
    DbSet<WorkflowExecutionPoint> WorkflowExecutionPoints { get; set; }
    DbSet<WorkflowInstance> WorkflowInstances { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}