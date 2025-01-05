using System.Text.Json.Serialization;

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
        public Connection(WorkflowStep source, WorkflowStep target)
        {
            Source = source;
            Target = target;
        }

        /// <summary>
        /// The source endpoint.
        /// </summary>
        public WorkflowStep Source { get; set; } = default!;

        /// <summary>
        /// The target endpoint.
        /// </summary>
        public WorkflowStep Target { get; set; } = default!;
    }
}
