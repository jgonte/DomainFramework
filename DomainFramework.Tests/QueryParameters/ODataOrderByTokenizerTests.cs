using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainFramework.Tests
{
    [TestClass]
    public class ODataOrderByTokenizerTests
    {
        [TestMethod]
        public void ODataOrderByTokenizer_Field_Name_Test()
        {
            ODataOrderByTokenizer tokenizer = new ODataOrderByTokenizer("Field1".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataOrderByTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataOrderByTokenizer_Field_Name_With_Leading_Spaces_Test()
        {
            ODataOrderByTokenizer tokenizer = new ODataOrderByTokenizer("     Field1".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataOrderByTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataOrderByTokenizer_Ascending_Test()
        {
            ODataOrderByTokenizer tokenizer = new ODataOrderByTokenizer("aSc".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataOrderByTokenizerTokenTypes.SortOrder, type);

            Assert.AreEqual("asc", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataOrderByTokenizer_Ascending_With_Leading_Spaces_Test()
        {
            ODataOrderByTokenizer tokenizer = new ODataOrderByTokenizer("   asC".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataOrderByTokenizerTokenTypes.SortOrder, type);

            Assert.AreEqual("asc", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataOrderByTokenizer_Field_Name_Ascending_Test()
        {
            ODataOrderByTokenizer tokenizer = new ODataOrderByTokenizer("Field1 asc".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataOrderByTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataOrderByTokenizerTokenTypes.SortOrder, type);

            Assert.AreEqual("asc", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataOrderByTokenizer_Field_Name_Descending_With_Leading_Spaces_Test()
        {
            ODataOrderByTokenizer tokenizer = new ODataOrderByTokenizer("    Field1 deSC".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataOrderByTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataOrderByTokenizerTokenTypes.SortOrder, type);

            Assert.AreEqual("desc", value);

            Assert.IsTrue(tokenizer.End());
        }
    }

}