using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.OutSelectorTest
{
    [TestClass]
    public partial class OutSelectorTest
    {
        [TestMethod]
        public void OutSelector_Traverses_Without_Edge_Label_When_Label_Not_Given()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<User>()
                .Out<Login>()
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','User').out().has('label','Login')", result);
        }

        [TestMethod]
        public void OutSelector_Traverses_With_Edge_Label_When_Label_Given()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<User>()
                .Out<Login>("unit test edge label")
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','User').out('unit test edge label').has('label','Login')", result);
        }

        [TestMethod]
        public void OutSelector_Adds_Alias_When_Alias_Given()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<User>()
                .Out<Login>("unit test edge label")
                .As("unit test alias")
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual(
                "g.V().has('label','User').out('unit test edge label').has('label','Login').as('unit test alias')",
                result);
        }

    }
}