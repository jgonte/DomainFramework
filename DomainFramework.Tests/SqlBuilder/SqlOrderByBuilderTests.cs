using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    [TestClass]
    public class SqlOrderByBuilderTests
    {
        [TestMethod]
        public void SqlOrderByBuilder_Single_Field_Ascending_Tests()
        {
            var sorters = new Queue<SorterNode>();

            sorters.Enqueue(new SorterNode
            {
                FieldName = "Field1",
                SortingOrder = SorterNode.SortingOrders.Ascending
            });
            
            var builder = new SqlOrderByBuilder(new SqlServerDatabaseDriver());

            Assert.AreEqual("Field1 ASC", builder.Build(sorters));
        }

        [TestMethod]
        public void SqlOrderByBuilder_Combination_Tests()
        {
            var sorters = new Queue<SorterNode>();

            sorters.Enqueue(new SorterNode
            {
                FieldName = "Field1",
                SortingOrder = SorterNode.SortingOrders.Ascending
            });

            sorters.Enqueue(new SorterNode
            {
                FieldName = "Field2",
                SortingOrder = SorterNode.SortingOrders.Descending
            });

            var builder = new SqlOrderByBuilder(new SqlServerDatabaseDriver());

            Assert.AreEqual("Field1 ASC, Field2 DESC", builder.Build(sorters));
        }
    }
}
