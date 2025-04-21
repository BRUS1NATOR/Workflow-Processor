// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;

public class UserTask
{
    [JsonIgnore]
    public WorkflowBookmark WorkflowBookmark { get; set; }

    [JsonPropertyName("workflowBookmarkId")]
    public long WorkflowBookmarkId { get; set; }

    [JsonPropertyName("userId")]
    public long UserId { get; set; }
}