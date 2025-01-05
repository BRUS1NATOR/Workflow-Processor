namespace WorkflowProcessor.Core.Connections
{
    public interface IConditionalConnection : IConnection
    {
        public bool Compare(object? value);
    }
}