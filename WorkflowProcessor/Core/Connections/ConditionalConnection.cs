using System.Text.Json.Serialization;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core.Connections.Metadata;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core.Connections
{
    public class ConditionalConnection<T> : Connection, IConditionalConnection where T : IComparable
    {
        public Func<T?, bool> Comparer { get; set; }

        [JsonConstructor]
        public ConditionalConnection()
        {

        }

        public ConditionalConnection(WorkflowStep<ExclusiveGateway<bool>> source, WorkflowStep target, T? value, IConnectionMetadata? metadata = null) : base(source, target, metadata)
        {
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<ExclusiveGateway<bool>> source, WorkflowStep target, Func<T?, bool> comparer, IConnectionMetadata? metadata = null) : base(source, target, metadata)
        {
            Comparer = comparer;
        }


        public ConditionalConnection(WorkflowStep<If> source, WorkflowStep target, T? value, IConnectionMetadata? metadata = null) : base(source, target, metadata)
        {
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<If> source, WorkflowStep target, Func<T?, bool> comparer, IConnectionMetadata? metadata = null) : base(source, target, metadata)
        {
            Comparer = comparer;
        }

        protected Func<T?, bool> DefaultComparer(T? value)
        {
            return x => x is null ? value == null : x.Equals(value);
        }

        public bool Compare(object? value)
        {
            if (value == null)
            {
                return Comparer.Invoke(default);
            }
            return Comparer.Invoke((T)value);
        }
    }

    public class ConditionalConnection<TContextData, T> : ConditionalConnection<T>, IConditionalConnection
        where T : IComparable
        where TContextData : IContextData, new()
    {

        public ConditionalConnection(WorkflowStep<ParallelExclusiveGateway<TContextData, T>> source, WorkflowStep target, T? value, IConnectionMetadata? metadata = null) 
        {
            Source = source;
            Target = target;
            SetMetadata(metadata);
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<ExclusiveGateway<TContextData, T>> source, WorkflowStep target, T? value, IConnectionMetadata? metadata = null)
        {
            Source = source;
            Target = target;
            SetMetadata(metadata);
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<ExclusiveGateway<TContextData, T>> source, WorkflowStep target, Func<T?, bool> comparer, IConnectionMetadata? metadata = null)
        {
            Source = source;
            Target = target;
            SetMetadata(metadata);
            Comparer = comparer;
        }

        public ConditionalConnection(WorkflowStep<If<TContextData>> source, WorkflowStep target, T? value, IConnectionMetadata? metadata = null)
        {
            Source = source;
            Target = target;
            SetMetadata(metadata);
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<If<TContextData>> source, WorkflowStep target, Func<T?, bool> comparer, IConnectionMetadata? metadata = null)
        {
            Source = source;
            Target = target;
            SetMetadata(metadata);
            Comparer = comparer;
        }
    }
}
