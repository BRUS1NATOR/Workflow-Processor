using System.Text.Json.Serialization;
using WorkflowProcessor.Core.Connections.Metadata;
using WorkflowProcessor.Core.Step;

namespace WorkflowProcessor.Core.Connections
{
    public class Connection : IConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        [JsonConstructor]
        public Connection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="source">The source endpoint.</param>
        /// <param name="target">The target endpoint.</param>
        public Connection(WorkflowStep source, WorkflowStep target, IConnectionMetadata? metadata = null)
        {
            Source = source;
            Target = target;
            SetMetadata(metadata);
        }

        /// <summary>
        /// Начальное событие
        /// </summary>
        [JsonPropertyName("source")]
        public WorkflowStep Source { get; set; } = default!;

        /// <summary>
        /// Конечное событие
        /// </summary>
        [JsonPropertyName("target")]
        public WorkflowStep Target { get; set; } = default!;

        [JsonPropertyName("metadata")]
        public IConnectionMetadata Metadata { get; set; }

        protected void SetMetadata(IConnectionMetadata? metadata)
        {
            if (metadata is null)
            {
                Metadata = new ConnectionMetadata();
                return;
            }
            Metadata = metadata;
        }
    }
}
