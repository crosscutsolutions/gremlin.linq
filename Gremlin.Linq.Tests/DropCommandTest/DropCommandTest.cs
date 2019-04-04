using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.DropCommandTest
{
    [TestClass]
    public class DropCommandTest
    {
        [TestMethod]
        public void DropCommand_Appends_Drop_To_Query()
        {
            // Arrange
            var client = new TestGraphClient();

            // Assert
            var result = client
                .From<User>()
                .Drop()
                .BuildGremlinQuery();

            // Act
            Assert.AreEqual("g.V().has('label','User').drop()", result);
        }
    }
}