using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainFramework.Tests
{
    [TestClass]
    public class ODataFilterTokenizerTests
    {
        [TestMethod]
        public void ODataFilterTokenizer_String_Literal_Single_Character_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("'A'".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.StringLiteral, type);

            Assert.AreEqual("A", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_String_Literal_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer(@"'123!@$%^&*()qwer <>,.?|\/'".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.StringLiteral, type);

            Assert.AreEqual(@"123!@$%^&*()qwer <>,.?|\/", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_String_Literal_With_Leading_Blanks_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer(@"   '123!@$%^&*()qwer <>,.?|\/'".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.StringLiteral, type);

            Assert.AreEqual(@"123!@$%^&*()qwer <>,.?|\/", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_String_Literal_With_Leading_Blanks_And_Ending_Parenthesis_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer(@"   '123!@$%^&*()qwer <>,.?|\/')".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.StringLiteral, type);

            Assert.AreEqual(@"123!@$%^&*()qwer <>,.?|\/", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ClosingParenthesis, type);

            Assert.AreEqual(")", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_String_Literal_With_Leading_And_Trailing_Blanks_And_Ending_Parenthesis_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer(@"   '123!@$%^&*()qwer <>,.?|\/' )".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.StringLiteral, type);

            Assert.AreEqual(@"123!@$%^&*()qwer <>,.?|\/", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ClosingParenthesis, type);

            Assert.AreEqual(")", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ODataFilterTokenizer_Unterminated_String_Literal_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer(@"'123!@$%^&*()qwer <>,.?|\/".ToCharArray());

            var (type, value) = tokenizer.GetToken();
        }

        [TestMethod]
        public void ODataFilterTokenizer_Numeric_Value_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("3213456".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.NumericValue, type);

            Assert.AreEqual("3213456", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Numeric_Value_With_Leading_Blanks_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("        3213456".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.NumericValue, type);

            Assert.AreEqual("3213456", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Numeric_Value_With_Leading_Blanks_And_Ending_Parenthesis_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("        3213456)".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.NumericValue, type);

            Assert.AreEqual("3213456", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ClosingParenthesis, type);

            Assert.AreEqual(")", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Numeric_Value_With_Leading_And_Trailing_Blanks_And_Ending_Parenthesis_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("        3213456         )".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.NumericValue, type);

            Assert.AreEqual("3213456", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ClosingParenthesis, type);

            Assert.AreEqual(")", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Boolean_True_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("TrUe".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.BooleanValue, type);

            Assert.AreEqual("true", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Boolean_True_With_Leading_Blanks_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer(" truE".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.BooleanValue, type);

            Assert.AreEqual("true", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Boolean_False_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("falSe".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.BooleanValue, type);

            Assert.AreEqual("false", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Boolean_False_Leading_Blanks_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("         FALse".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.BooleanValue, type);

            Assert.AreEqual("false", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Boolean_False_Leading_Blanks_And_Ending_Parenthesis_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("         FALse)".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.BooleanValue, type);

            Assert.AreEqual("false", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ClosingParenthesis, type);

            Assert.AreEqual(")", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Boolean_False_Leading_And_Trailing_Blanks_And_Ending_ParenthesisTest()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("         FALse  )".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.BooleanValue, type);

            Assert.AreEqual("false", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ClosingParenthesis, type);

            Assert.AreEqual(")", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Function_Name_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("ContAIns(".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FunctionName, type);

            Assert.AreEqual("contains", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Function_Name_With_Trailing_Spaces_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("  ContAIns      (".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FunctionName, type);

            Assert.AreEqual("contains", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Logical_Operator_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("aNd".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.LogicalOperator, type);

            Assert.AreEqual("and", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Logical_Operator_With_Opening_Parenthesis_After_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("aNd(".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.LogicalOperator, type);

            Assert.AreEqual("and", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Logical_Operator_With_Opening_Parenthesis_After_And_Spaces_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("   aNd  (".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.LogicalOperator, type);

            Assert.AreEqual("and", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Comparison_Operator_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("eQ".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ComparisonOperator, type);

            Assert.AreEqual("eq", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Comparison_Operator_With_Opening_Parenthesis_After_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("Eq(".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ComparisonOperator, type);

            Assert.AreEqual("eq", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Comparison_Operator_With_Opening_Parenthesis_After_And_Spaces_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("   EQ  (".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ComparisonOperator, type);

            Assert.AreEqual("eq", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Field_Name_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("Field1".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Opening_Parenthesis_With_Field_Name_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("(Field1".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Function_Name_With_Opening_Parenthesis_With_Field_Name_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("Contains(Field1".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FunctionName, type);

            Assert.AreEqual("contains", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Function_Name_With_Opening_Parenthesis_With_Field_Name_And_String_Literal_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("   Contains (  Field1 , 'Abc'  )".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FunctionName, type);

            Assert.AreEqual("contains", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.Comma, type);

            Assert.AreEqual(",", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.StringLiteral, type);

            Assert.AreEqual("Abc", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ClosingParenthesis, type);

            Assert.AreEqual(")", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Field_Name_With_Comparison_Operator_And_Numeric_Value_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("   Field2 eq   23".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field2", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ComparisonOperator, type);

            Assert.AreEqual("eq", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.NumericValue, type);

            Assert.AreEqual("23", value);

            Assert.IsTrue(tokenizer.End());
        }

        [TestMethod]
        public void ODataFilterTokenizer_Combination_Of_The_Two_Tests_Above_Test()
        {
            ODataFilterTokenizer tokenizer = new ODataFilterTokenizer("   Contains (  Field1 , 'Abc'  ) and    Field2 eq   23".ToCharArray());

            var (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FunctionName, type);

            Assert.AreEqual("contains", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.OpeningParenthesis, type);

            Assert.AreEqual("(", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field1", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.Comma, type);

            Assert.AreEqual(",", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.StringLiteral, type);

            Assert.AreEqual("Abc", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ClosingParenthesis, type);

            Assert.AreEqual(")", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.LogicalOperator, type);

            Assert.AreEqual("and", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.FieldName, type);

            Assert.AreEqual("Field2", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.ComparisonOperator, type);

            Assert.AreEqual("eq", value);

            (type, value) = tokenizer.GetToken();

            Assert.AreEqual(ODataFilterTokenizerTokenTypes.NumericValue, type);

            Assert.AreEqual("23", value);

            Assert.IsTrue(tokenizer.End());


        }
    }
}
