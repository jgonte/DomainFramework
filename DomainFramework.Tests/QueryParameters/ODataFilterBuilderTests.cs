using DomainFramework.Core;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DomainFramework.Tests
{
    [TestClass]
    public class ODataFilterBuilderTests
    {
        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Equals_Numeric_Value_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" Field1 Eq 12");

            var filterNode = (SingleValueFieldFilterNode)filter.Single();

            Assert.AreEqual("Field1", filterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, filterNode.Operator);

            Assert.AreEqual(12, filterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Equals_String_Literal_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" Field1 Eq 'ab12'    ");

            var filterNode = (SingleValueFieldFilterNode)filter.Single();

            Assert.AreEqual("Field1", filterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, filterNode.Operator);

            Assert.AreEqual("ab12", filterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Equals_Boolean_Value_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" Field1 Eq true");

            var filterNode = (SingleValueFieldFilterNode)filter.Single();

            Assert.AreEqual("Field1", filterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, filterNode.Operator);

            Assert.AreEqual(true, filterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Contains_String_Literal_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" ContainS  (  Field1,   'ABC'  )");

            var filterNode = (TwoParametersFunctionCallFilterNode)filter.Single();

            Assert.AreEqual("Field1", filterNode.FieldName);

            Assert.AreEqual("LIKE", filterNode.Name);

            Assert.AreEqual("ABC", filterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Contains_Numeric_Value_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" ContaiNS  (  Field1,   12  )");

            var filterNode = (TwoParametersFunctionCallFilterNode)filter.Single();

            Assert.AreEqual("Field1", filterNode.FieldName);

            Assert.AreEqual("LIKE", filterNode.Name);

            Assert.AreEqual(12, filterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Contains_Boolean_Value_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" ContaiNS  (  Field1,   false  )");

            var filterNode = (TwoParametersFunctionCallFilterNode)filter.Single();

            Assert.AreEqual("Field1", filterNode.FieldName);

            Assert.AreEqual(FilterNode.Like, filterNode.Name);

            Assert.AreEqual(false, filterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Contains_Numeric_Value_And_Equals_String_Literal_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" ContaiNS  (  Field1,   12  ) and  Field2 Eq 'ab12'   ");

            var functionCallFilterNode = (TwoParametersFunctionCallFilterNode)filter.ElementAt(0);

            Assert.AreEqual("Field1", functionCallFilterNode.FieldName);

            Assert.AreEqual(FilterNode.Like, functionCallFilterNode.Name);

            Assert.AreEqual(12, functionCallFilterNode.FieldValue);

            var andFilterNode = filter.ElementAt(1);

            Assert.AreEqual(FilterNode.And, andFilterNode);

            var singleValueFieldFilterNode = (SingleValueFieldFilterNode)filter.ElementAt(2);

            Assert.AreEqual("Field2", singleValueFieldFilterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, singleValueFieldFilterNode.Operator);

            Assert.AreEqual("ab12", singleValueFieldFilterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Contains_Numeric_Value_And_Equals_String_Literal_And_Field3_Equals_True_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" ContaiNS  (  Field1,   12  ) and  Field2 Eq 'ab12' And Field3 eQ true  ");

            var functionCallFilterNode = (TwoParametersFunctionCallFilterNode)filter.ElementAt(0);

            Assert.AreEqual("Field1", functionCallFilterNode.FieldName);

            Assert.AreEqual(FilterNode.Like, functionCallFilterNode.Name);

            Assert.AreEqual(12, functionCallFilterNode.FieldValue);

            var andFilterNode = filter.ElementAt(1);

            Assert.AreEqual(FilterNode.And, andFilterNode);

            var singleValueFieldFilterNode = (SingleValueFieldFilterNode)filter.ElementAt(2);

            Assert.AreEqual("Field2", singleValueFieldFilterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, singleValueFieldFilterNode.Operator);

            Assert.AreEqual("ab12", singleValueFieldFilterNode.FieldValue);

            andFilterNode = filter.ElementAt(3);

            Assert.AreEqual(FilterNode.And, andFilterNode);

            singleValueFieldFilterNode = (SingleValueFieldFilterNode)filter.ElementAt(4);

            Assert.AreEqual("Field3", singleValueFieldFilterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, singleValueFieldFilterNode.Operator);

            Assert.AreEqual(true, singleValueFieldFilterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Contains_Numeric_Value_And_Equals_String_Literal_Or_Field3_Equals_True_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" ContaiNS  (  Field1,   12  ) and  Field2 Eq 'ab12' oR  Not  Field3 eQ true  ");

            var functionCallFilterNode = (TwoParametersFunctionCallFilterNode)filter.ElementAt(0);

            Assert.AreEqual("Field1", functionCallFilterNode.FieldName);

            Assert.AreEqual(FilterNode.Like, functionCallFilterNode.Name);

            Assert.AreEqual(12, functionCallFilterNode.FieldValue);

            var andFilterNode = filter.ElementAt(1);

            Assert.AreEqual(FilterNode.And, andFilterNode);

            var singleValueFieldFilterNode = (SingleValueFieldFilterNode)filter.ElementAt(2);

            Assert.AreEqual("Field2", singleValueFieldFilterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, singleValueFieldFilterNode.Operator);

            Assert.AreEqual("ab12", singleValueFieldFilterNode.FieldValue);

            var orFilterNode = filter.ElementAt(3);

            Assert.AreEqual(FilterNode.Or, orFilterNode);

            var notFilterNode = filter.ElementAt(4);

            Assert.AreEqual(FilterNode.Not, notFilterNode);

            singleValueFieldFilterNode = (SingleValueFieldFilterNode)filter.ElementAt(5);

            Assert.AreEqual("Field3", singleValueFieldFilterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, singleValueFieldFilterNode.Operator);

            Assert.AreEqual(true, singleValueFieldFilterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Contains_Numeric_Value_Or_Equals_String_Literal_And_Field3_Equals_True_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build(" nOt ( ContaiNS  (  Field1,   12  ) Or  Field2 Eq 'ab12' ) aNd   Field3 eQ    true  ");

            var notFilterNode = filter.ElementAt(0);

            Assert.AreEqual(FilterNode.Not, notFilterNode);

            var beginGroupingFilterNode = filter.ElementAt(1);

            Assert.AreEqual(FilterNode.BeginGrouping, beginGroupingFilterNode);

            var functionCallFilterNode = (TwoParametersFunctionCallFilterNode)filter.ElementAt(2);

            Assert.AreEqual("Field1", functionCallFilterNode.FieldName);

            Assert.AreEqual(FilterNode.Like, functionCallFilterNode.Name);

            Assert.AreEqual(12, functionCallFilterNode.FieldValue);

            var orFilterNode = filter.ElementAt(3);

            Assert.AreEqual(FilterNode.Or, orFilterNode);

            var singleValueFieldFilterNode = (SingleValueFieldFilterNode)filter.ElementAt(4);

            Assert.AreEqual("Field2", singleValueFieldFilterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, singleValueFieldFilterNode.Operator);

            Assert.AreEqual("ab12", singleValueFieldFilterNode.FieldValue);

            var endGroupingFilterNode = filter.ElementAt(5);

            Assert.AreEqual(FilterNode.EndGrouping, endGroupingFilterNode);

            var andFilterNode = filter.ElementAt(6);

            Assert.AreEqual(FilterNode.And, andFilterNode);

            singleValueFieldFilterNode = (SingleValueFieldFilterNode)filter.ElementAt(7);

            Assert.AreEqual("Field3", singleValueFieldFilterNode.FieldName);

            Assert.AreEqual(FilterNode.IsEqual, singleValueFieldFilterNode.Operator);

            Assert.AreEqual(true, singleValueFieldFilterNode.FieldValue);
        }

        [TestMethod]
        public void ODataFilterBuilder_Field_Filter_Contains_String_Literal_And_Contains_String_Literal_Test()
        {
            ODataFilterBuilder builder = new ODataFilterBuilder();

            var filter = builder.Build("not (contains(firstName, 'wei') and contains(lastName, 'j'))");

            var notFilterNode = filter.ElementAt(0);

            Assert.AreEqual(FilterNode.Not, notFilterNode);

            var beginGroupingFilterNode = filter.ElementAt(1);

            Assert.AreEqual(FilterNode.BeginGrouping, beginGroupingFilterNode);

            var functionCallFilterNode = (TwoParametersFunctionCallFilterNode)filter.ElementAt(2);

            Assert.AreEqual("firstName", functionCallFilterNode.FieldName);

            Assert.AreEqual(FilterNode.Like, functionCallFilterNode.Name);

            Assert.AreEqual("wei", functionCallFilterNode.FieldValue);

            var andFilterNode = filter.ElementAt(3);

            Assert.AreEqual(FilterNode.And, andFilterNode);

            functionCallFilterNode = (TwoParametersFunctionCallFilterNode)filter.ElementAt(4);

            Assert.AreEqual("lastName", functionCallFilterNode.FieldName);

            Assert.AreEqual(FilterNode.Like, functionCallFilterNode.Name);

            Assert.AreEqual("j", functionCallFilterNode.FieldValue);

            var endGroupingFilterNode = filter.ElementAt(5);

            Assert.AreEqual(FilterNode.EndGrouping, endGroupingFilterNode);

        }

    }
}
