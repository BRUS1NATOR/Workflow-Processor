using System.Text.Json.Serialization;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core
{
    public class Workflow : WorkflowInfo
    {
        [JsonPropertyName("initialContext")]
        public Context InitialContext { get; set; }

        [JsonPropertyName("scheme")]
        public WorkflowScheme Scheme { get; set; } = new();

        public Workflow()
        {

        }

        public Workflow(string name, int version)
        {
            Name = name;
            Version = version;
        }
    }
}