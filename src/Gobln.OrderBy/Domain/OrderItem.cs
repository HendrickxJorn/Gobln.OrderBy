using Gobln.Interface;

namespace Gobln.Domain
{
    /// <summary>
    /// Contains the information about the order object
    /// </summary>
    public class OrderItem : IOrderItem
    {
        /// <summary>
        /// <see cref="OrderDirection"/>
        /// </summary>
        public OrderDirectionEnum OrderDirection { get; set; } = OrderDirectionEnum.Ascending;

        /// <summary>
        /// Name of the colum to sort by
        /// </summary>
        public string SortColum { get; set; } = string.Empty;
    }

    /// <summary>
    /// The order information and compare information
    /// </summary>
    public class OrderItem<TKey> : OrderItem, IOrderItem<TKey>
    {
        /// <summary>
        /// Define Compare
        /// </summary>
        public System.Collections.Generic.IComparer<TKey> Comparer { get; set; }
    }
}