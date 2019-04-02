using Gremlin.Linq.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.WhereAnySelectorTest
{
    public partial class WhereAnySelectorTest
    {
        [TestMethod]
        public void WhereAnySelector_Quotes_Value_When_Property_Is_String()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .Where("unit test property", "unit test value")
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('unit test property','unit test value')", result);
        }

        [TestMethod]
        public void WhereAnySelector_Does_Not_Quote_Value_When_Property_Is_Integer()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .Where("unit test property", 99)
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('unit test property', 99)", result);
        }

        [TestMethod]
        public void WhereAnySelector_Does_Not_Quote_Value_When_Property_Is_Boolean()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .Where("unit test property", true)
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('unit test property', true)", result);
        }
    }
}