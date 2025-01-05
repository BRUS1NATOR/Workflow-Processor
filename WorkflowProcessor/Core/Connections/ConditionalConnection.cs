using WorkflowProcessor.Activities;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Core.Connections
{
    public class ConditionalConnection<T> : Connection, IConditionalConnection where T : IComparable
    {
        public Func<T?, bool> Comparer { get; set; }

        public ConditionalConnection()
        {
            
        }

        public ConditionalConnection(WorkflowStep<Gateway<bool>> source, WorkflowStep target, T? value)
        {
            Source = source;
            Target = target;
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<Gateway<bool>> source, WorkflowStep target, Func<T?, bool> comparer)
        {
            Source = source;
            Target = target;
            Comparer = comparer;
        }


        public ConditionalConnection(WorkflowStep<If> source, WorkflowStep target, T? value)
        {
            Source = source;
            Target = target;
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<If> source, WorkflowStep target, Func<T?, bool> comparer)
        {
            Source = source;
            Target = target;
            Comparer = comparer;
        }

        public ConditionalConnection(WorkflowStep<UserActivity> source, WorkflowStep target, T? value)
        {
            Source = source;
            Target = target;
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<UserActivity> source, WorkflowStep target, Func<T?, bool> comparer)
        {
            Source = source;
            Target = target;
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
    public class ConditionalConnection<TContext, T> : ConditionalConnection<T>, IConditionalConnection where T : IComparable where TContext : IContextData, new()
    {
        public ConditionalConnection(WorkflowStep<Gateway<TContext, T>> source, WorkflowStep target, T? value)
        {
            Source = source;
            Target = target;
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<Gateway<TContext, T>> source, WorkflowStep target, Func<T?, bool> comparer)
        {
            Source = source;
            Target = target;
            Comparer = comparer;
        }


        public ConditionalConnection(WorkflowStep<If<TContext>> source, WorkflowStep target, T? value)
        {
            Source = source;
            Target = target;
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<If<TContext>> source, WorkflowStep target, Func<T?, bool> comparer)
        {
            Source = source;
            Target = target;
            Comparer = comparer;
        }


        public ConditionalConnection(WorkflowStep<UserActivity<TContext>> source, WorkflowStep target, T? value)
        {
            Source = source;
            Target = target;
            Comparer = DefaultComparer(value);
        }

        public ConditionalConnection(WorkflowStep<UserActivity<TContext>> source, WorkflowStep target, Func<T?, bool> comparer)
        {
            Source = source;
            Target = target;
            Comparer = comparer;
        }
    }
}
