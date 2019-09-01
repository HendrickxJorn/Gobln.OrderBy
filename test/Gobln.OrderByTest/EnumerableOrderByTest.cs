using Gobln.Comparer;
using Gobln.Domain;
using Gobln.OrderBy;
using Gobln.OrderByTest.Comparer;
using Gobln.OrderByTest.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Gobln.OrderByTest
{
    [TestClass]
    public class EnumerableOrderByTest
    {
        [TestMethod]
        public void OrderByTestWithStringTest1()
        {
            // order by FirstName Descending then by LunarEvaDate
            var list = TestData.MoonWalkers.OrderBy(" firstname       Desc; LunarEvaDate asc ; ").ToList();

            var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.FirstName).ThenBy(c => c.LunarEvaDate).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByTest1()
        {
            // orderby LunarEvaDate ascending
            var list = TestData.MoonWalkers.OrderBy("LunarEvaDate").ToArray();

            var listExpected = TestData.MoonWalkers.OrderBy(c => c.LunarEvaDate).ToArray();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByTest2()
        {
            // orderby FirstName ascending
            var list = TestData.MoonWalkers.OrderBy("FirstName", false).ToList();

            var listExpected = TestData.MoonWalkers.OrderBy(c => c.FirstName).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByTest3()
        {
            // orderby LunarEvaDate ascending
            var list = TestData.MoonWalkers.OrderBy("lunarevadate", "asc").ToList();

            var listExpected = TestData.MoonWalkers.OrderBy(c => c.LunarEvaDate).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByTest4()
        {
            // orderby LunarEvaDate descending
            var list = TestData.MoonWalkers.OrderBy("LunarEvaDate", OrderDirectionEnum.Descending).ToList();

            var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LunarEvaDate).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByTest5()
        {
            // orderby LastName descending
            var list = TestData.MoonWalkers.OrderBy("LastName", true).ToList();

            var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LastName).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByTest6()
        {
            // orderby LastName descending
            var list = TestData.MoonWalkers.OrderBy(new OrderItem() { SortColum = "LastName", OrderDirection = OrderDirectionEnum.Descending }).ToList();

            var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LastName).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByNaturalSortComparerTest1()
        {
            var list = TestData.MoonWalkers.OrderBy("GuiId", OrderDirectionEnum.Ascending, new NaturalSortComparer()).ToList();

            var listExpected = TestData.MoonWalkers.OrderBy(c => c.GuiId, new NaturalSortComparer()).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByNaturalSortComparerTest2()
        {
            var orderItem =
                new OrderItem<string>()
                {
                    SortColum = "guiid",
                    Comparer = new NaturalSortComparer()
                };

            var list = TestData.MoonWalkers.OrderBy(orderItem).ToList();

            var listExpected = TestData.MoonWalkers.OrderBy(c => c.GuiId, new NaturalSortComparer()).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidCastException), "")]
        public void OrderByNaturalSortComparerTest3()
        {
            // will give an System.InvalidCastException
            // NaturalSortComparer is of type string and id is of type int
            TestData.MoonWalkers.OrderBy("Id", OrderDirectionEnum.Ascending, new NaturalSortComparer()).ToList();
        }

        [TestMethod]
        public void OrderByNaturalSortComparerTest4()
        {
            var list = TestData.MoonWalkers.OrderBy(c => c.FirstName).ThenBy("GuiId", OrderDirectionEnum.Ascending, new NaturalSortComparer()).ToList();

            var listExpected = TestData.MoonWalkers.OrderBy(c => c.FirstName).ThenBy(c => c.GuiId, new NaturalSortComparer()).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }

        [TestMethod]
        public void OrderByOrderItemTest1()
        {
            var orderItems = new List<OrderItem>{
                    new OrderItem(){ SortColum = "LunarEvaDate", OrderDirection = OrderDirectionEnum.Descending },
                    new OrderItem(){ SortColum = "Remark" }
                };

            var list = TestData.MoonWalkers.OrderBy(orderItems).ToList();

            var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LunarEvaDate).ThenBy(c => c.Remark).ToList();

            CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
        }
    }
}