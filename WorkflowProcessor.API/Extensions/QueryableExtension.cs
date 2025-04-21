using System.Linq.Expressions;

namespace WorkflowProcessor.API.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> @this,
            bool condition,
            Expression<Func<T, bool>> @where)
        {
            return condition ? @this.Where(@where) : @this;
        }
    }
}
