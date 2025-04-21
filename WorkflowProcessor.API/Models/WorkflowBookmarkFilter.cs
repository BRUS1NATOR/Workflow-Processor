using WorkflowProcessor.Core.Enums;

namespace WorkflowProcessor.API.Models
{
    public class WorkflowBookmarkFilter
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public WorkflowBookmarkType? Type { get; set; }
        public WorkflowBookmarkStatus? Status { get; set; }
        public string? Name { get; set; }
    }
}