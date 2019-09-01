using Gobln.Domain;
using System;
using System.Linq;

namespace Gobln.OrderBy.Infrastructure
{
    internal class Direction
    {
        private readonly string[] Ascending = { "a", "asc", "ascending", "OrderBy", "0", "up" };
        private readonly string[] Descending = { "d", "desc", "descending", "OrderByDescending", "1", "down" };

        internal OrderDirectionEnum Get(string sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortDirection))
            {
                throw new ArgumentNullException("sortDirection");
            }

            if (!(Ascending.Contains(sortDirection.ToLower()) || Descending.Contains(sortDirection.ToLower())))
            {
                throw new ArgumentException("SortDirection is not of \"a\", \"asc\", \"ascending\", \"OrderBy\", \"0\", \"up\", \"d\", \"desc\", \"descending\", \"OrderByDescending\", \"1\", \"down\".");
            }

            return Get(!string.IsNullOrWhiteSpace(sortDirection) && Descending.Contains(sortDirection.ToLower()));
        }

        internal OrderDirectionEnum Get(bool isDescending) => isDescending ? OrderDirectionEnum.Descending : OrderDirectionEnum.Ascending;
    }
}