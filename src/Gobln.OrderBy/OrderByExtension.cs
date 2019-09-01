using Gobln.Domain;
using Gobln.Infrastructure;
using Gobln.Interface;
using Gobln.OrderBy.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gobln.OrderBy
{
    /// <summary>
    /// Orderby extension
    /// </summary>
    public static class OrderbyExtension
    {
        #region Orderby

        #region IEnumerable

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string sortColum)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return source.OrderBy(Converter.ConvertStringToOrderItemList(sortColum));
        }

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="sortDirection">the order by which to sort</param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string sortColum, string sortDirection)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            if (string.IsNullOrWhiteSpace(sortDirection))
                throw new ArgumentException("Input cannot be null or empty", "sortDirection");

            return OrderByInternal.EnumerableOrder(source, new OrderItem { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(sortDirection) }, out _);
        }

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string sortColum, bool isDescending)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.EnumerableOrder(source, new OrderItem { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(isDescending) }, out _);
        }

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string sortColum, OrderDirectionEnum orderDirection)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.EnumerableOrder(source, new OrderItem { SortColum = sortColum, OrderDirection = orderDirection }, out _);
        }

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitem"><see cref="IOrderItem"/></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, IOrderItem orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentException("Input cannot be null or empty", "orderitem.SortColum");

            return OrderByInternal.EnumerableOrder(source, orderitem, out _);
        }

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitems"><see cref="IEnumerable{IOrderItem}"/></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, IEnumerable<IOrderItem> orderitems)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (!orderitems.Any())
                throw new ArgumentNullException("orderitems");

            if (orderitems.Any(c => string.IsNullOrWhiteSpace(c.SortColum)))
                throw new ArgumentException("Input cannot be null or empty", "orderitems.SortColum");

            var isOrderby = true;
            var processed = false;
            //Can not convert source to IOrderedQueryable
            //  => gives error
            IOrderedEnumerable<TSource> returnOrder = null;

            foreach (var orderitem in orderitems)
            {
                returnOrder = isOrderby
                    ? OrderByInternal.EnumerableOrder(source, orderitem, out processed)
                    : OrderByInternal.EnumerableOrderThenBy(returnOrder, orderitem);

                if (isOrderby && processed)
                    isOrderby = false;
            }

            return returnOrder;
        }

        #region Comparer

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <param name="comparer"><see cref="IComparer{TKey}" /></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, string sortColum, bool isDescending, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.EnumerableOrder(source, new OrderItem<TKey> { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(isDescending), Comparer = comparer }, out _);
        }

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        /// <param name="comparer"><see cref="IComparer{TKey}" /></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, string sortColum, OrderDirectionEnum orderDirection, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.EnumerableOrder(source, new OrderItem<TKey> { SortColum = sortColum, OrderDirection = orderDirection, Comparer = comparer }, out _);
        }

        /// <summary>
        /// Order an <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitem"><see cref="IOrderItem"/></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, OrderItem<TKey> orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentException("Input cannot be null or empty", "orderitem.SortColum");

            return OrderByInternal.EnumerableOrder(source, orderitem, out _);
        }

        #endregion Comparer

        #endregion IEnumerable

        #region IQueryable

        /// <summary>
        /// Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string sortColum)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentNullException("sortColum");

            if (string.IsNullOrWhiteSpace(sortColum))
                return (IOrderedQueryable<TSource>)source;

            return source.OrderBy(Converter.ConvertStringToOrderItemList(sortColum));
        }

        /// <summary>
        /// Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="sortDirection">the order by which to sort</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string sortColum, string sortDirection)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.QueryableOrder(source, new OrderItem { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(sortDirection) }, out _);
        }

        /// <summary>
        ///  Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string sortColum, bool isDescending)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.QueryableOrder(source, new OrderItem { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(isDescending) }, out _);
        }

        /// <summary>
        /// Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string sortColum, OrderDirectionEnum orderDirection)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.QueryableOrder(source, new OrderItem { SortColum = sortColum, OrderDirection = orderDirection }, out _);
        }

        /// <summary>
        /// Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitem"><see cref="IOrderItem"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, IOrderItem orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.QueryableOrder(source, orderitem, out _);
        }

        /// <summary>
        /// Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitems"><see cref="IEnumerable{IOrderItem}"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, IEnumerable<IOrderItem> orderitems)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (!orderitems.Any())
                return (IOrderedQueryable<TSource>)source;

            var isOrderby = true;
            var processed = false;
            //Can not convert source to IOrderedQueryable
            //  => gives error
            IOrderedQueryable<TSource> returnOrder = null;

            foreach (var orderitem in orderitems)
            {
                returnOrder = isOrderby
                    ? OrderByInternal.QueryableOrder(source, orderitem, out processed)
                    : OrderByInternal.QueryableOrderThenBy(returnOrder, orderitem);

                if (isOrderby && processed)
                    isOrderby = false;
            }

            return returnOrder;
        }

        #region Comparer

        /// <summary>
        /// Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <param name="comparer"><see cref="IComparer{TKey}" /></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, string sortColum, bool isDescending, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.QueryableOrder(source, new OrderItem<TKey> { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(isDescending), Comparer = comparer }, out _);
        }

        /// <summary>
        /// Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        /// <param name="comparer"><see cref="IComparer{TKey}" /></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, string sortColum, OrderDirectionEnum orderDirection, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.QueryableOrder(source, new OrderItem<TKey> { SortColum = sortColum, OrderDirection = orderDirection, Comparer = comparer }, out _);
        }

        /// <summary>
        /// Order an <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitem"><see cref="IOrderItem"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, OrderItem<TKey> orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentException("Input cannot be null or empty", "orderitem.SortColum");

            return OrderByInternal.QueryableOrder(source, orderitem, out _);
        }

        #endregion Comparer

        #endregion IQueryable

        #endregion Orderby

        #region ThenBy

        #region IOrderedEnumerable

        /// <summary>
        /// Order <see cref="IOrderedEnumerable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="sortDirection">the order by which to sort</param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, string sortColum, string sortDirection)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.EnumerableOrderThenBy(source, new OrderItem { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(sortDirection) });
        }

        /// <summary>
        /// Order <see cref="IOrderedEnumerable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, string sortColum, bool isDescending)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.EnumerableOrderThenBy(source, new OrderItem { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(isDescending) });
        }

        /// <summary>
        /// Order <see cref="IOrderedEnumerable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, string sortColum, OrderDirectionEnum orderDirection)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.EnumerableOrderThenBy(source, new OrderItem { SortColum = sortColum, OrderDirection = orderDirection });
        }

        /// <summary>
        /// Order <see cref="IOrderedEnumerable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitem"><see cref="IOrderItem"/></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, IOrderItem orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.EnumerableOrderThenBy(source, orderitem);
        }

        /// <summary>
        /// Order <see cref="IOrderedEnumerable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitems"><see cref="IEnumerable{IOrderItem}"/></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, IEnumerable<IOrderItem> orderitems)
        {
            if (orderitems == null)
                throw new ArgumentNullException("orderitems");

            var returnOrder = source;

            foreach (var orderitem in orderitems)
                returnOrder = OrderByInternal.EnumerableOrderThenBy(returnOrder, orderitem);

            return returnOrder;
        }

        #region Comparer

        /// <summary>
        /// Order <see cref="IOrderedEnumerable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <param name="comparer"><see cref="IComparer{TKey}" /></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, string sortColum, bool isDescending, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.EnumerableOrderThenBy(source, new OrderItem<TKey> { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(isDescending), Comparer = comparer });
        }

        /// <summary>
        /// Order <see cref="IOrderedEnumerable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        /// <param name="comparer"><see cref="IComparer{TKey}" /></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, string sortColum, OrderDirectionEnum orderDirection, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.EnumerableOrderThenBy(source, new OrderItem<TKey> { SortColum = sortColum, OrderDirection = orderDirection, Comparer = comparer });
        }

        /// <summary>
        /// Order <see cref="IOrderedEnumerable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitem"><see cref="IOrderItem"/></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, OrderItem<TKey> orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentException("Input cannot be null or empty", "orderitem.SortColum");

            return OrderByInternal.EnumerableOrderThenBy(source, orderitem);
        }

        #endregion Comparer

        #endregion IOrderedEnumerable

        #region IOrderedQueryable

        /// <summary>
        /// Order <see cref="IOrderedQueryable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="sortDirection">the order by which to sort</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, string sortColum, string sortDirection)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.QueryableOrderThenBy(source, new OrderItem { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(sortDirection) });
        }

        /// <summary>
        /// Order <see cref="IOrderedQueryable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, string sortColum, bool isDescending)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.QueryableOrderThenBy(source, new OrderItem { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(isDescending) });
        }

        /// <summary>
        /// Order <see cref="IOrderedQueryable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">name of the column to sort on</param>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, string sortColum, OrderDirectionEnum orderDirection)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.QueryableOrderThenBy(source, new OrderItem { SortColum = sortColum, OrderDirection = orderDirection });
        }

        /// <summary>
        /// Order <see cref="IOrderedQueryable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitem"><see cref="IOrderItem"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, IOrderItem orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return OrderByInternal.QueryableOrderThenBy(source, orderitem);
        }

        /// <summary>
        /// Order <see cref="IOrderedQueryable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitems"><see cref="IEnumerable{IOrderItem}"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, IEnumerable<IOrderItem> orderitems)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (orderitems == null)
                throw new ArgumentNullException("orderitems");

            var returnOrder = source;

            foreach (var orderitem in orderitems)
                returnOrder = OrderByInternal.QueryableOrderThenBy(returnOrder, orderitem);

            return returnOrder;
        }

        #region Comparer

        /// <summary>
        /// Order <see cref="IOrderedQueryable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum">Comma or semicolumn seperated string expression of the colume and direction. Directon is not mandatory, if not pressend then it will be ascending</param>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <param name="comparer"><see cref="IComparer{TKey}" /></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, string sortColum, bool isDescending, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.QueryableOrderThenBy(source, new OrderItem<TKey> { SortColum = sortColum, OrderDirection = Orderby.GetOrderDirection(isDescending), Comparer = comparer });
        }

        /// <summary>
        /// Order <see cref="IOrderedQueryable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortColum"></param>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        /// <param name="comparer"><see cref="IComparer{TKey}" /></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, string sortColum, OrderDirectionEnum orderDirection, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(sortColum))
                throw new ArgumentException("Input cannot be null or empty", "sortColum");

            return OrderByInternal.QueryableOrderThenBy(source, new OrderItem<TKey> { SortColum = sortColum, OrderDirection = orderDirection, Comparer = comparer });
        }

        /// <summary>
        /// Order <see cref="IOrderedQueryable{TSource}"/> then by
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderitem"><see cref="IOrderItem"/></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, OrderItem<TKey> orderitem)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrWhiteSpace(orderitem.SortColum))
                throw new ArgumentException("Input cannot be null or empty", "orderitem.SortColum");

            return OrderByInternal.QueryableOrderThenBy(source, orderitem);
        }

        #endregion Comparer

        #endregion IOrderedQueryable

        #endregion ThenBy
    }
}