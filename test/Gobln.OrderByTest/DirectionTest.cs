using Gobln.OrderBy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gobln.OrderByTest
{
    [TestClass]
    public class DirectionTest
    {
        [TestMethod]
        public void OrderByTestGetOrderDirection()
        {
            var isdescending = Orderby.GetOrderDirection("desc");

            isdescending = Orderby.GetOrderDirection("1");

            isdescending = Orderby.GetOrderDirection("d");

            var isAscending = Orderby.GetOrderDirection("asc");

            isAscending = Orderby.GetOrderDirection("0");

            isAscending = Orderby.GetOrderDirection("a");
        }
    }
}
