using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.WhereSelectorTest
{
    public partial class WhereSelectorTest
    {
        [TestMethod]
        public void Where_Quotes_Value_When_Property_Is_String_And_Operator_Is_Equality()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .Where(a => a.StringProperty == "unit test value")
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('sp', 'unit test value')", result);
        }

        [TestMethod]
        public void Where_Does_Not_Quote_Value_When_Property_Is_Integer_And_Operator_Is_Equality()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .Where(a => a.IntegerProperty == 99)
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('ip', 99)", result);
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        public void Where_Does_Not_Quote_Value_When_Property_Is_Boolean_And_Operator_Is_Equality()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .Where(a => a.BooleanProperty)
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('bp', true)", result);
        }

        [TestMethod]
        public void Where_Quotes_Value_When_Property_Is_String_And_Expression_Is_Equals_Method()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .Where(a => a.StringProperty.Equals("unit test value"))
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('sp', 'unit test value')", result);
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        public void Where_Does_Not_Quote_Value_When_Property_Is_Integer_And_Expression_Is_Equals_Method()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .Where(a => a.IntegerProperty.Equals(99))
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('ip', 99)", result);
        }
    }
}