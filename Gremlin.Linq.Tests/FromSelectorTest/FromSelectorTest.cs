using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.FromSelectorTest
{
    [TestClass]
    public partial class FromSelectorTest
    {
        [TestMethod]
        public void FromSelector_Uses_Class_Name_When_GremlinLabelAttribute_Is_Not_Present()
        {
            // Arrange
            var client = new TestGraphClient();

            // Assert
            var result = client
                .From<User>()
                .BuildGremlinQuery();

            // Act
            Assert.AreEqual("g.V().has('label','User')", result);
        }

        [TestMethod]
        public void FromSelector_Uses_Given_Vertex_Label_When_GremlinLabelAttribute_Is_Present()
        {
            // Arrange
            var client = new TestGraphClient();

            // Assert
            var result = client
                .From<GremlinLabelTest>()
                .BuildGremlinQuery();

            // Act
            Assert.AreEqual("g.V().has('label','gl')", result);
        }
    }
}