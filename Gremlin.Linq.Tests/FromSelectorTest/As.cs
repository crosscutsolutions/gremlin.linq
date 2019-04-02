using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.FromSelectorTest
{
    public partial class FromSelectorTest
    {
        [TestMethod]
        public void As_Appends_Alias_To_Query()
        {
            // Arrange
            var client = new TestGraphClient();

            // Assert
            var result = client
                .From<GremlinLabelTest>()
                .As("unit test alias")
                .BuildGremlinQuery();

            // Act
            Assert.AreEqual("g.V().has('label','gl').as('unit test alias')", result);
        }
    }
}