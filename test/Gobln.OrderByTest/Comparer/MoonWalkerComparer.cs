using Gobln.OrderByTest.Model;
using System.Collections.Generic;

namespace Gobln.OrderByTest.Comparer
{
    public class MoonWalkerComparer : Comparer<MoonWalker>
    {
        public override int Compare(MoonWalker x, MoonWalker y)
        {
            if ((x.FirstName ?? string.Empty).CompareTo((y.FirstName ?? string.Empty)) != 0)
            {
                return (x.FirstName ?? string.Empty).CompareTo((y.FirstName ?? string.Empty));
            }
            else if (x.Id.CompareTo(y.Id) != 0)
            {
                return x.Id.CompareTo(y.Id);
            }
            else if ((x.LastName ?? string.Empty).CompareTo((y.LastName ?? string.Empty)) != 0)
            {
                return (x.LastName ?? string.Empty).CompareTo((y.LastName ?? string.Empty));
            }
            else if (x.LunarEvaDate.CompareTo(y.LunarEvaDate) != 0)
            {
                return x.LunarEvaDate.CompareTo(y.LunarEvaDate);
            }
            else if ((x.Remark ?? string.Empty).CompareTo((y.Remark ?? string.Empty)) != 0)
            {
                return (x.Remark ?? string.Empty).CompareTo((y.Remark ?? string.Empty));
            }

            return 0;
        }
    }
}