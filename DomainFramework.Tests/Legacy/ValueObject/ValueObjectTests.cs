using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainFramework.Tests
{
    [TestClass]
    public class ValueObjectTests
    {
        [TestMethod]
        public void Value_Object_Equality_Test()
        {
            var oneDollar = new Money(1);
            var anotherDollar = new Money(1);

            Assert.AreEqual(oneDollar, anotherDollar);
        }
    }
}
