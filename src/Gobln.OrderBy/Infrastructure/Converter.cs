using Gobln.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gobln.OrderBy.Infrastructure
{
    internal static class Converter
    {
        internal static IEnumerable<OrderItem> ConvertStringToOrderItemList(string sortColum)
        {
            var orderitems = new List<OrderItem>();

            var tempSortColumn = Regex.Replace(
                    sortColum
                        .Replace(';', ',')
                        .Trim(',')
                        .Trim()
                    , @"\s+", " ");

            foreach (var item in tempSortColumn.Split(',').Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                var sort = item.Trim().Split(' ');

                orderitems.Add(new OrderItem
                {
                    SortColum = sort[0],
                    OrderDirection = sort.Count() > 1
                        ? new Direction().Get(sort[1])
                        : OrderDirectionEnum.Ascending
                });
            }

            return orderitems;
        }
    }
}