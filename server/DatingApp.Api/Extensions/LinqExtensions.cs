using System.Linq.Expressions;

namespace DatingApp.Api.Extensions
{
    public static class LinqExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool isAscending)
        {
            return isAscending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }
    }
}