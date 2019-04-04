using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.WhereSelectorTest
{
    [TestClass]
    public partial class WhereSelectorTest
    {
        [TestMethod]
        public void Where_Uses_Class_Name_When_GremlinPropertyAttribute_Is_Not_Present()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<User>()
                .Where(u => u.FirstName == "unit test name")
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','User').has('FirstName', 'unit test name')", result);
        }

        [TestMethod]
        public void Where_Uses_Given_Vertex_Label_When_GremlinPropertyAttribute_Is_Present()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<GremlinPropertyTest>()
                .Where(u => u.StringProperty == "unit test value")
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','GremlinPropertyTest').has('sp', 'unit test value')", result);
        }
    }
}