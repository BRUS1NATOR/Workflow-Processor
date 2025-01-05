// See https://aka.ms/new-console-template for more information
using WorkflowProcessor.Core.Enums;

public class WorkflowBookmark
{
    public int Id { get; set; }
    //
    public string? Name { get; set; }

    public WorkflowBookmarkStatus Status { get; set; }
    //
    public WorkflowExecutionPoint WorkflowExecutionPoint { get; set; }
    public int WorkflowExecutionPointId { get; set; }
    //

    public List<UserTask> UserTasks { get; set; }
}
