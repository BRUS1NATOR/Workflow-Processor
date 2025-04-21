using System.Text.Json.Serialization;

namespace WorkflowProcessor.API.Models
{
    public class ReplaceExecutor
    {
        [JsonPropertyName("bookmarkId")]
        public long BookmarkId { get; set; }

        [JsonPropertyName("oldExecutor")]
        public long OldExecutor { get; set; }

        [JsonPropertyName("newExecutor")]
        public long NewExecutor { get; set; }
    }
}
