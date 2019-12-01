using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    [TestClass]
    public class SqlBuilderTests
    {
        [TestMethod]
        public void SqlBuilder_Function_Call_Contains_Tests()
        {
            var filter = new Queue<FilterNode>();

            filter.Enqueue(new ContainsFunctionCallFilterNode
            {
                FieldName = "Field1",
                FieldValue = "abc"
            });
            
            var builder = new SqlFilterBuilder(new SqlServerDatabaseDriver());

            Assert.AreEqual("Field1 LIKE '%abc%'", builder.Build(filter));
        }

        [TestMethod]
        public void SqlBuilder_Single_Value_Field_Equals_Tests()
        {
            var filter = new Queue<FilterNode>();

            filter.Enqueue(new SingleValueFieldFilterNode
            {
                FieldName = "Field1",
                Operator = FilterNode.IsEqual,
                FieldValue = "abc"
            });

            var builder = new SqlFilterBuilder(new SqlServerDatabaseDriver());

            Assert.AreEqual("Field1  =  'abc'", builder.Build(filter));
        }

        [TestMethod]
        public void SqlBuilder_Multi_Value_Field_In_Tests()
        {
            var filter = new Queue<FilterNode>();

            filter.Enqueue(new InOperatorFilterNode
            {
                FieldName = "Field1",
                FieldValues = new List<object>
                {
                    100,
                    2000
                }
            });

            var builder = new SqlFilterBuilder(new SqlServerDatabaseDriver());

            Assert.AreEqual("Field1 IN (100, 2000)", builder.Build(filter));
        }

        [TestMethod]
        public void SqlBuilder_Not_Multi_Value_Field_In_Tests()
        {
            var filter = new Queue<FilterNode>();

            filter.Enqueue(FilterNode.Not);

            filter.Enqueue(new InOperatorFilterNode
            {
                FieldName = "Field1",
                FieldValues = new List<object>
                {
                    100,
                    2000
                }
            });

            var builder = new SqlFilterBuilder(new SqlServerDatabaseDriver());

            Assert.AreEqual(" NOT Field1 IN (100, 2000)", builder.Build(filter));
        }
    }
}
