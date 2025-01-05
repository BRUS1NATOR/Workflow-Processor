// See https://aka.ms/new-console-template for more information
public class UserTask
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string DisplayName { get; set; }
    public WorkflowBookmark WorkflowBookmark { get; set; }
    public int WorkflowBookmarkId { get; set; }
    public UserTaskMetadata Metadata { get; set; }
}
