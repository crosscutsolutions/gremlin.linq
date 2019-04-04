using System;
using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.ConnectedVertexSelectorTest
{
    [TestClass]
    public class ConnectedVertexSelectorTest
    {
        [TestMethod]
        public void ConnectedVertexSelector_Traverses_Using_Edge_Label()
        {
            // Arrange
            var client = new TestGraphClient();

            // Act
            var result = client
                .From<User>()
                .SelectOut<Login>("unit test edge label")
                .BuildGremlinQuery();

            // Assert
            Assert.AreEqual("g.V().has('label','User').out('unit test edge label').hasLabel('Login')", result);
        }

    }
}