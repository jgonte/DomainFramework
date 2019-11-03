using DomainFramework.Core;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DomainFramework.Tests
{
    [TestClass]
    public class ODataOrderByBuilderTests
    {
        [TestMethod]
        public void ODataOrderByBuilder_Field_OrderBy_Ascending_Test()
        {
            ODataOrderByBuilder builder = new ODataOrderByBuilder();

            var sorters = builder.Build(" Field1 ASC");

            var sorterNode = sorters.Single();

            Assert.AreEqual("Field1", sorterNode.FieldName);

            Assert.AreEqual(SorterNode.SortingOrders.Ascending, sorterNode.SortingOrder);
        }

        [TestMethod]
        public void ODataOrderByBuilder_Field_OrderBy_Descending_Test()
        {
            ODataOrderByBuilder builder = new ODataOrderByBuilder();

            var sorters = builder.Build(" Field2 deSC");

            var sorterNode = sorters.Single();

            Assert.AreEqual("Field2", sorterNode.FieldName);

            Assert.AreEqual(SorterNode.SortingOrders.Descending, sorterNode.SortingOrder);
        }

        [TestMethod]
        public void ODataOrderByBuilder_Combination_Of_Above_Test()
        {
            ODataOrderByBuilder builder = new ODataOrderByBuilder();

            var sorters = builder.Build(" Field1 ASC ,  Field2 deSC");

            var sorterNode = sorters.First();

            Assert.AreEqual("Field1", sorterNode.FieldName);

            Assert.AreEqual(SorterNode.SortingOrders.Ascending, sorterNode.SortingOrder);

            sorterNode = sorters.Last();

            Assert.AreEqual("Field2", sorterNode.FieldName);

            Assert.AreEqual(SorterNode.SortingOrders.Descending, sorterNode.SortingOrder);
        }

    }
}
