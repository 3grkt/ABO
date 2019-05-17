using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ABO.Core
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Sorts IQueryable object using sort expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            // DataSource control passes the sort parameter with a direction
            // if the direction is descending          
            string[] arr = propertyName.Split(' ');
            string sortField = arr[0];
            string sorDir = arr[1];

            if (string.IsNullOrEmpty(sortField))
            {
                return source;
            }

            string[] fields = sortField.Split('.');

            ParameterExpression parameter = Expression.Parameter(source.ElementType, string.Empty);
            MemberExpression property = Expression.Property(parameter, fields[0]); // there is at least 1 item in arrays => no out of range exception
            if (fields.Length > 1)
            {
                for (int i = 1; i < fields.Length; i++)
                {
                    property = Expression.Property(property, fields[i]);
                }
            }
            LambdaExpression lambda = Expression.Lambda(property, parameter);
            string methodName = sorDir.Equals("ASC", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            Expression methodCallExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { source.ElementType, property.Type },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        /// <summary>
        /// Gets end of date point.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndOfDate(this DateTime dt)
        {
            return CommonHelper.GetEndOfDate(dt);
        }

        /// <summary>
        /// Trims string to get only {numOfWords} characters.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="numOfWords"></param>
        /// <param name="getFullWord"></param>
        /// <returns></returns>
        public static string Trim(this string s, int numOfWords, bool getFullWord = false)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (s.Length <= numOfWords)
                return s;

            if (getFullWord)
            {
                var lastSpace = s.LastIndexOf(' ', numOfWords);
                if (lastSpace >= 0)
                    return s.Substring(0, lastSpace) + "...";
            }

            return s.Substring(0, numOfWords) + "...";
        }
    }
}
