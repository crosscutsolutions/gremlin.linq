using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.WhereInSelectorTest
{
    [TestClass]
    public class WhereInSelectorTest
    {
        [TestMethod]
        public void WhereIn_Uses_Class_Name_When_GremlinLabel_Not_Present()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<User>()
                .WhereIn(x => x.FirstName, new[] {"a"})
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','User').has('FirstName',within('a'))", result);
        }

        [TestMethod]
        public void WhereIn_Uses_GremlinProperty_Label_When_Present()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .WhereIn(x => x.StringProperty, new[] {"a"})
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('sp',within('a'))", result);
        }

        [TestMethod]
        public void WhereIn_Uses_Quoted_Values_When_Property_Is_String()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .WhereIn(x => x.StringProperty, new[] {"a", "b"})
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('sp',within('a','b'))", result);
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        public void WhereIn_Uses_Quoted_Values_When_Property_Is_Integer()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .WhereIn(x => x.IntegerProperty, new object[] {1, 2})
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('sp',within(1, 2))", result);
        }
    }
}