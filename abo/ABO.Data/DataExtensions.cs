using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ABO.Data
{
    public static class DataExtensions
    {
        public static IQueryable<T> IncludeTable<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> path)
        {
            return query.Include(path);
        }
    }
}
