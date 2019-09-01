using Gobln.Domain;

namespace Gobln.Interface
{
    /// <summary>
    /// The order information
    /// </summary>
    public interface IOrderItem
    {
        /// <summary>
        /// <see cref="OrderDirection"/>
        /// </summary>
        OrderDirectionEnum OrderDirection { get; set; }

        /// <summary>
        /// Name of the colum to sort by
        /// </summary>
        string SortColum { get; set; }
    }

    /// <summary>
    /// The order information and compare information
    /// </summary>
    public interface IOrderItem<TKey> : IOrderItem
    {
        /// <summary>
        /// Define Compare
        /// </summary>
        System.Collections.Generic.IComparer<TKey> Comparer { get; set; }
    }
}