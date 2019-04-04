using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.OutSelectorTest
{
    public partial class OutSelectorTest
    {
        [TestMethod]
        public void As_Adds_Alias_When_Alias_Inferred_By_Type()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<User>()
                .Out<Login>("unit test edge label")
                .As<string>()
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual(
                "g.V().has('label','User').out('unit test edge label').has('label','Login').as('String')",
                result);
        }

        [TestMethod]
        public void As_Uses_GremlinLabel_When_Present()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<User>()
                .Out<Login>("unit test edge label")
                .As<GremlinLabelTest>()
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual(
                "g.V().has('label','User').out('unit test edge label').has('label','Login').as('gl')",
                result);
        }
    }
}