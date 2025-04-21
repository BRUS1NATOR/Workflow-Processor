using System.Text.Json.Serialization;
using WorkflowProcessor.Activities;
using WorkflowProcessor.Core.Connections.Metadata;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core.Connections
{

    public class UserConnection<TContextData> : Connection, IUserConnection
        where TContextData : IContextData, new()
    {
        [JsonPropertyName("connector")]
        public string Connector { get; set; }

        private Func<TContextData?, bool>? Comparer;

        public UserConnection(WorkflowStep<UserActivity<TContextData>> source, WorkflowStep target, string value, IConnectionMetadata? metadata = null)
        {
            Connector = value;
            Source = source;
            Target = target;
            SetMetadata(metadata);
        }

        public UserConnection(WorkflowStep<UserActivity<TContextData>> source, WorkflowStep target, string value, Func<TContextData?, bool> comparer, IConnectionMetadata? metadata = null)
        {
            Connector = value;
            Source = source;
            Target = target;
            SetMetadata(metadata);
            Comparer = comparer;
        }


        public bool Compare(string connector, object contextData)
        {
            // Сначала проверка на переход 
            if (connector != Connector)
            {
                return false;
            }
            if (Comparer is null)
            {
                return true;
            }
            return Comparer.Invoke((TContextData)contextData);
        }
    }
}
