using Gobln.Domain;
using Gobln.OrderBy;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gobln.Comparer
{
    /// <summary>
    /// Sort Alphanumeric as Alphanumeric and numeric as numeric
    /// </summary>
    public class NaturalSortComparer : IComparer<string>, IDisposable
    {
        private readonly OrderDirectionEnum _orderDirection;
        private Dictionary<string, string[]> _dic = new Dictionary<string, string[]>();
        private const string spliter = "([0-9]+)";

        /// <summary>
        /// Sort Alphanumeric as Alphanumeric and numeric as numeric
        /// </summary>
        public NaturalSortComparer()
        {
            _orderDirection = OrderDirectionEnum.Ascending;
        }

        /// <summary>
        /// Sort Alphanumeric as Alphanumeric and numeric as numeric
        /// </summary>
        /// <param name="isDescending">Is the order direction descending</param>
        public NaturalSortComparer(bool isDescending)
        {
            _orderDirection = Orderby.GetOrderDirection(isDescending);
        }

        /// <summary>
        /// Sort Alphanumeric as Alphanumeric and numeric as numeric
        /// </summary>
        /// <param name="orderDirection"><see cref="OrderDirectionEnum"/></param>
        public NaturalSortComparer(OrderDirectionEnum orderDirection)
        {
            _orderDirection = orderDirection;
        }

        #region IComparer<string> Members

        int IComparer<string>.Compare(string x, string y)
        {
            if (x == y)
            {
                return 0;
            }

            var returnVal = 0;

            if (x == null || y == null)
            {
                if (x == null)
                {
                    returnVal = -1;
                }
                else if (y == null)
                {
                    returnVal = 1;
                }
            }
            else
            {
                if (!_dic.TryGetValue(x, out string[] tempX))
                {
                    tempX = Regex.Split(x.Replace(" ", string.Empty), spliter);
                    _dic.Add(x, tempX);
                }

                if (!_dic.TryGetValue(y, out string[] tempY))
                {
                    tempY = Regex.Split(y.Replace(" ", string.Empty), spliter);
                    _dic.Add(y, tempY);
                }

                for (var i = 0; i < tempX.Length && i < tempY.Length; i++)
                {
                    if (tempX[i] == tempY[i])
                        continue;

                    returnVal = PartCompare(tempX[i], tempY[i]);

                    return _orderDirection == OrderDirectionEnum.Ascending
                        ? returnVal
                        : -returnVal;
                }

                if (tempY.Length > tempX.Length)
                {
                    returnVal = 1;
                }
                else if (tempX.Length > tempY.Length)
                {
                    returnVal = -1;
                }
            }

            return _orderDirection == OrderDirectionEnum.Ascending
                ? returnVal
                : -returnVal;
        }

        private static int PartCompare(string left, string right)
        {
            if (!int.TryParse(left, out int x) || !int.TryParse(right, out int y))
                return string.CompareOrdinal(left, right);

            return x.CompareTo(y);
        }

        #endregion IComparer<string> Members

        /// <summary>
        /// Dispose of NaturalSortComparer
        /// </summary>
        public void Dispose()
        {
            _dic.Clear();
            _dic = null;
        }
    }
}