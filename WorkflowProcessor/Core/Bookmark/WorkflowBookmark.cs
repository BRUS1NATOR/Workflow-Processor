// See https://aka.ms/new-console-template for more information
using System.Text.Json.Serialization;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Enums;

public class WorkflowBookmark
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Users that allow to complete bookmark
    /// </summary>
    [JsonPropertyName("userTasks")]
    public List<UserTask> UserTasks { get; set; }

    [JsonPropertyName("status")]
    public WorkflowBookmarkStatus Status { get; set; }

    [JsonPropertyName("type")]
    public WorkflowBookmarkType Type { get; set; }
    //
    [JsonPropertyName("workflowExecutionPointId")]
    public long WorkflowExecutionPointId { get; set; }

    [JsonPropertyName("workflowExecutionPoint")]
    public WorkflowExecutionPoint WorkflowExecutionPoint { get; set; }
    //

    [JsonPropertyName("workflowChildId")]
    public long? WorkflowChildId { get; set; }
    [JsonIgnore]
    public WorkflowInstance? WorkflowChild { get; set; }
}
