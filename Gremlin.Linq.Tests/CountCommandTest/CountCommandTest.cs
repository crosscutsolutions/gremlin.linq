using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.CountCommandTest
{
    [TestClass]
    public class CountCommandTest
    {
        [TestMethod]
        public void CountCommand_Appends_Count_To_Query()
        {
            // Arrange
            var client = new TestGraphClient();

            // Assert
            var result = client
                .From<User>()
                .Count()
                .BuildGremlinQuery();

            // Act
            Assert.AreEqual("g.V().has('label','User').count()", result);
        }
    }
}