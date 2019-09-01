using Gobln.Domain;
using Gobln.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Gobln.Infrastructure
{
    internal static class OrderByInternal
    {
        #region Enumerable OrderBy

        internal static IOrderedEnumerable<TSource> EnumerableOrder<TSource>(IEnumerable<TSource> source, IOrderItem orderitem, out bool processed)
        {
            processed = false;

            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentNullException("sortColum");

            var prop = GetOrderByFunc<TSource, object>(orderitem.SortColum);

            if (prop == null)
                throw new ArgumentException("sortColum");

            processed = true;

            return orderitem.OrderDirection == OrderDirectionEnum.Ascending
                       ? Enumerable.OrderBy(source, prop)
                       : Enumerable.OrderByDescending(source, prop);
        }

        internal static IOrderedEnumerable<TSource> EnumerableOrder<TSource, TKey>(IEnumerable<TSource> source, IOrderItem<TKey> orderitem, out bool processed)
        {
            processed = false;

            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentNullException("sortColum");

            var prop = GetOrderByFunc<TSource, TKey>(orderitem.SortColum, true);

            if (prop == null)
                throw new ArgumentException("sortColum");

            processed = true;

            return orderitem.OrderDirection == OrderDirectionEnum.Ascending
                   ? Enumerable.OrderBy(source, prop, orderitem.Comparer)
                   : Enumerable.OrderByDescending(source, prop, orderitem.Comparer);
        }

        #endregion Enumerable OrderBy

        #region Enumerable OrderThenBy

        internal static IOrderedEnumerable<TSource> EnumerableOrderThenBy<TSource>(IOrderedEnumerable<TSource> source, IOrderItem orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentNullException("sortColum");

            var prop = GetOrderByFunc<TSource, object>(orderitem.SortColum);

            if (prop == null)
                throw new ArgumentException("sortColum");

            return orderitem.OrderDirection == OrderDirectionEnum.Ascending
                       ? Enumerable.ThenBy(source, prop)
                       : Enumerable.ThenByDescending(source, prop);
        }

        internal static IOrderedEnumerable<TSource> EnumerableOrderThenBy<TSource, TKey>(IOrderedEnumerable<TSource> source, IOrderItem<TKey> orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentNullException("sortColum");

            var prop = GetOrderByFunc<TSource, TKey>(orderitem.SortColum, true);

            if (prop == null)
                throw new ArgumentException("sortColum");

            return orderitem.OrderDirection == OrderDirectionEnum.Ascending
                   ? Enumerable.ThenBy(source, prop, orderitem.Comparer)
                   : Enumerable.ThenByDescending(source, prop, orderitem.Comparer);
        }

        #endregion Enumerable OrderThenBy

        #region Queryable OrderBy

        internal static IOrderedQueryable<TSource> QueryableOrder<TSource>(IQueryable<TSource> source, IOrderItem orderitem, out bool processed)
        {
            processed = false;

            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentNullException("sortColum");

            var type = typeof(TSource);
            var arg = Expression.Parameter(type, "x");

            var prop = type.GetProperty(orderitem.SortColum, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop == null)
                throw new ArgumentException("sortColum");

            var expr = Expression.Property((Expression)arg, prop);
            type = prop.PropertyType;

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), type);

            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable)
                .GetMethods()
                .Single(c => c.Name == (orderitem.OrderDirection == OrderDirectionEnum.Ascending ? "OrderBy" : "OrderByDescending")
                            && c.IsGenericMethodDefinition
                            && c.GetGenericArguments().Length == 2
                            && c.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TSource), type)
                .Invoke(null, new object[] { source, lambda });

            processed = true;

            return (IOrderedQueryable<TSource>)result;
        }

        internal static IOrderedQueryable<TSource> QueryableOrder<TSource, TKey>(IQueryable<TSource> source, IOrderItem<TKey> orderitem, out bool processed)
        {
            processed = false;

            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentNullException("sortColum");

            var type = typeof(TSource);
            var arg = Expression.Parameter(type, "x");

            var prop = type.GetProperty(orderitem.SortColum, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
                throw new ArgumentException("sortColum");

            var expr = Expression.Property((Expression)arg, prop);
            type = prop.PropertyType;

            if (prop.PropertyType != typeof(TKey))
            {
                throw new System.InvalidCastException($"Unable to cast object of type '{prop.PropertyType.ToString()}' to type '{typeof(TKey).ToString()}'.");
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), type);

            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable)
                .GetMethods()
                .Single(c => c.Name == (orderitem.OrderDirection == OrderDirectionEnum.Ascending ? "OrderBy" : "OrderByDescending")
                            && c.IsGenericMethodDefinition
                            && c.GetGenericArguments().Length == 2
                            && c.GetParameters().Length == 3)
                .MakeGenericMethod(typeof(TSource), type)
                .Invoke(null, new object[] { source, lambda, orderitem.Comparer });

            processed = true;

            return (IOrderedQueryable<TSource>)result;
        }

        #endregion Queryable OrderBy

        #region Queryable OrderThenBy

        internal static IOrderedQueryable<TSource> QueryableOrderThenBy<TSource>(IQueryable<TSource> source, IOrderItem orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentNullException("sortColum");

            var type = typeof(TSource);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            var prop = type.GetProperty(orderitem.SortColum);
            if (prop == null)
                throw new ArgumentException("sortColum");

            expr = Expression.Property(expr, prop);
            type = prop.PropertyType;

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods()
                    .Single(c => c.Name == (orderitem.OrderDirection == OrderDirectionEnum.Ascending ? "ThenBy" : "ThenByDescending")
                            && c.IsGenericMethodDefinition
                            && c.GetGenericArguments().Length == 2
                            && c.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TSource), type)
                    .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }

        internal static IOrderedQueryable<TSource> QueryableOrderThenBy<TSource, TKey>(IQueryable<TSource> source, IOrderItem<TKey> orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentNullException("sortColum");

            var type = typeof(TSource);
            var arg = Expression.Parameter(type, "x");

            var prop = type.GetProperty(orderitem.SortColum, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
                throw new ArgumentException("sortColum");

            var expr = Expression.Property((Expression)arg, prop);
            type = prop.PropertyType;

            if (prop.PropertyType != typeof(TKey))
            {
                throw new System.InvalidCastException($"Unable to cast object of type '{prop.PropertyType.ToString()}' to type '{typeof(TKey).ToString()}'.");
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), type);

            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable)
                .GetMethods()
                .Single(c => c.Name == (orderitem.OrderDirection == OrderDirectionEnum.Ascending ? "ThenBy" : "ThenByDescending")
                            && c.IsGenericMethodDefinition
                            && c.GetGenericArguments().Length == 2
                            && c.GetParameters().Length == 3)
                .MakeGenericMethod(typeof(TSource), type)
                .Invoke(null, new object[] { source, lambda, orderitem.Comparer });

            return (IOrderedQueryable<TSource>)result;
        }

        #endregion Queryable OrderThenBy

        private static Func<TSource, TKey> GetOrderByFunc<TSource, TKey>(string sortColumn, bool checkType = false)
        {
            Func<TSource, TKey> orderByFunc = null;
            if (!string.IsNullOrEmpty(sortColumn))
            {
                var type = typeof(TSource);

                if (type.GetProperties().Any(c => c.Name.Equals(sortColumn, StringComparison.OrdinalIgnoreCase)))
                {
                    var prop = type.GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (!checkType || (checkType &&  prop.PropertyType == typeof(TKey)))
                    {
                        orderByFunc = data => (TKey)prop.GetValue(data, null);
                    }
                    else
                    {
                        throw new System.InvalidCastException($"Unable to cast object of type '{prop.PropertyType.ToString()}' to type '{typeof(TKey).ToString()}'.");
                    }
                }
            }

            return orderByFunc;
        }


    }
}