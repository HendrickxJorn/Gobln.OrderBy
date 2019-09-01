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
    public class EnumerableOrderByDbTest
    {
        [TestMethod]
        public void OrderByDbTestWithStringTest1()
        {
            using (var context = new MoonWalkerContext())
            {
                // order by FirstName Descending then by LunarEvaDate
                var list = context.MoonWalkers.OrderBy(" firstname       Desc; LunarEvaDate asc ; ").ToList();

                var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.FirstName).ThenBy(c => c.LunarEvaDate).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        public void OrderByDbTest1()
        {
            using (var context = new MoonWalkerContext())
            {
                // orderby LunarEvaDate ascending
                var list = context.MoonWalkers.OrderBy("LunarEvaDate").ToArray();

                var listExpected = TestData.MoonWalkers.OrderBy(c => c.LunarEvaDate).ToArray();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        public void OrderByDbTest2()
        {
            using (var context = new MoonWalkerContext())
            {
                // orderby FirstName ascending
                var list = context.MoonWalkers.OrderBy("FirstName", false).ToList();

                var listExpected = TestData.MoonWalkers.OrderBy(c => c.FirstName).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        public void OrderByDbTest3()
        {
            using (var context = new MoonWalkerContext())
            {
                // orderby LunarEvaDate ascending
                var list = context.MoonWalkers.OrderBy("lunarevadate", "asc").ToList();

                var listExpected = TestData.MoonWalkers.OrderBy(c => c.LunarEvaDate).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        public void OrderByDbTest4()
        {
            using (var context = new MoonWalkerContext())
            {
                // orderby LunarEvaDate descending
                var list = context.MoonWalkers.OrderBy("LunarEvaDate", OrderDirectionEnum.Descending).ToList();

                var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LunarEvaDate).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        public void OrderByDbTest5()
        {
            using (var context = new MoonWalkerContext())
            {
                // orderby LastName descending
                var list = context.MoonWalkers.OrderBy("LastName", true).ToList();

                var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LastName).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        public void OrderByDbTest6()
        {
            using (var context = new MoonWalkerContext())
            {
                // orderby LunarEvaDate descending
                var list = context.MoonWalkers.OrderBy("LunarEvaDate", "descending").ToList();

                var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LunarEvaDate).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        public void OrderByDbTest7()
        {
            using (var context = new MoonWalkerContext())
            {
                // orderby LastName descending
                var list = context.MoonWalkers.OrderBy(new OrderItem() { SortColum = "LastName", OrderDirection = OrderDirectionEnum.Descending }).ToList();

                var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LastName).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.NotSupportedException), "")]
        public void OrderByDbNaturalSortComparerTest1()
        {
            using (var context = new MoonWalkerContext())
            {
                // Will not work, Comparer is not supported in EF or linq-to-sql
                var list = context.MoonWalkers.OrderBy("GuiId", false, new NaturalSortComparer()).ToList();

                var listExpected = TestData.MoonWalkers.OrderBy(c => c.GuiId, new NaturalSortComparer()).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.NotSupportedException), "")]
        public void OrderByDbNaturalSortComparerTest2()
        {
            using (var context = new MoonWalkerContext())
            {
                var orderItem =
                    new OrderItem<string>()
                    {
                        SortColum = "guiid",
                        Comparer = new NaturalSortComparer()
                    };

                // Will not work, Comparer is not supported in EF or linq-to-sql
                var list = context.MoonWalkers.OrderBy(orderItem).ToList();

                var listExpected = TestData.MoonWalkers.OrderBy(c => c.Remark, new NaturalSortComparer()).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
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
        public void OrderByDbOrderItemTest1()
        {
            using (var context = new MoonWalkerContext())
            {
                var orderItems = new List<OrderItem>{
                    new OrderItem(){ SortColum = "LunarEvaDate", OrderDirection = OrderDirectionEnum.Descending },
                    new OrderItem(){ SortColum = "Remark" }
                };

                var list = context.MoonWalkers.OrderBy(orderItems).ToList();

                var listExpected = TestData.MoonWalkers.OrderByDescending(c => c.LunarEvaDate).ThenBy(c => c.Remark).ToList();

                CollectionAssert.AreEqual(listExpected, list, new MoonWalkerComparer());
            }
        }
    }
}