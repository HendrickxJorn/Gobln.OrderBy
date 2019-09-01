using Gobln.Domain;
using Gobln.OrderBy.Infrastructure;

namespace Gobln.OrderBy
{
    /// <summary>
    /// Helper class for the orderby extension
    /// </summary>
    public static class Orderby
    {
        /// <summary>
        /// Convert string to <see cref="OrderDirectionEnum"/>
        /// </summary>
        /// <param name="sortDirection">The order by which to sort</param>
        /// <returns><see cref="OrderDirectionEnum"/>Return <see cref="OrderDirectionEnum.Ascending"/> if string is in "a", "asc", "ascending", "OrderBy", "0", "up".
        ///     Or return <see cref="OrderDirectionEnum.Descending"/> if string is in "d", "desc", "descending", "OrderByDescending", "1", "down".
        ///     If not found then return <see cref="OrderDirectionEnum.Ascending"/>.
        /// </returns>
        public static OrderDirectionEnum GetOrderDirection(string sortDirection) => new Direction().Get(sortDirection);

        /// <summary>
        /// Get the <see cref="OrderDirectionEnum"/>
        /// </summary>
        /// <param name="isDescending">Is the order direction descending</param>
        /// <returns><see cref="OrderDirectionEnum"/>Return <see cref="OrderDirectionEnum.Ascending"/> if true.
        ///     Else return <see cref="OrderDirectionEnum.Descending"/>.
        /// </returns>
        public static OrderDirectionEnum GetOrderDirection(bool isDescending) => new Direction().Get(isDescending);
    }
}