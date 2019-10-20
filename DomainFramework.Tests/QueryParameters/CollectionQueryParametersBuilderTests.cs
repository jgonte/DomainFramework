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
        public void Empty_Query_Parameters_Test()
        {
            var queryParameters = new CollectionQueryParametersBuilder(new QueryCollection())
                .Build();

            Assert.IsNull(queryParameters.Filter);
        }

        [TestMethod]
        public void Empty_Query_Parameters_Single_Value_Field_Filter()
        {
            var queryParameters = new CollectionQueryParametersBuilder(new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "$filter", "contains(Field1, 'ABC')" }
                }))
                .Build();

            var filter = queryParameters.Filter;

            Assert.IsInstanceOfType(filter.Single(), typeof(ContainsFunctionCallFilterNode));
        }
    }
}
