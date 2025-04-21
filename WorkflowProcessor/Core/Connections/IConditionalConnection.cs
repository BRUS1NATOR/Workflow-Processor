namespace WorkflowProcessor.Core.Connections
{
    public interface IConditionalConnection : IConnection
    {
        public bool Compare(object? value);
    }

    public interface IUserConnection : IConnection
    {
        public string Connector { get; set; }
        public bool Compare(string connector, object context);
    }
}