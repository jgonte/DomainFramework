using DomainFramework.Core;
using DomainFramework.DataAccess;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    [TestClass]
    public class CollectionQueryParametersBuilderTests
    {
        [TestMethod]
        public void CollectionQueryParametersBuilder_Empty_Query_Parameters_Test()
        {
            var queryParameters = new CollectionQueryParametersBuilder(new QueryCollection())
                .Build();

            Assert.IsNull(queryParameters.Filter);
        }

        [TestMethod]
        public void CollectionQueryParametersBuilder_Select_Test()
        {
            var queryParameters = new CollectionQueryParametersBuilder(new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "$select", "field1  ,  field2" }
                }))
                .Build();

            var fields = queryParameters.Select;

            Assert.AreEqual("field1", fields.First());

            Assert.AreEqual("field2", fields.Last());
        }

        [TestMethod]
        public void CollectionQueryParametersBuilder_Query_Parameters_Single_Filter_Test()
        {
            var queryParameters = new CollectionQueryParametersBuilder(new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "$filter", "contains(Field1, 'ABC')" }
                }))
                .Build();

            var filter = queryParameters.Filter;

            Assert.IsInstanceOfType(filter.Single(), typeof(ContainsFunctionCallFilterNode));
        }

        [TestMethod]
        public void CollectionQueryParametersBuilder_Query_Parameters_Single_Sorter_Test()
        {
            var queryParameters = new CollectionQueryParametersBuilder(new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "$orderby", "Field1 ASC" }
                }))
                .Build();

            var sorters = queryParameters.OrderBy;

            Assert.IsInstanceOfType(sorters.Single(), typeof(SorterNode));
        }
    }
}
