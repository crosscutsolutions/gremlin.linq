using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.GremlinPropertyAttributeTest
{
    [TestClass]
    public class GremlinPropertyAttributeTest
    {
        [TestMethod]
        public void Ctor_Throws_ArgumentException_When_Name_Is_Null()
        {
            // Arrange

            // Assert

            // Act
            Assert.ThrowsException<ArgumentException>(() => new GremlinPropertyAttribute(null));
        }

        [TestMethod]
        public void Ctor_Throws_ArgumentException_When_Name_Is_Empty()
        {
            // Arrange

            // Assert

            // Act
            Assert.ThrowsException<ArgumentException>(() => new GremlinPropertyAttribute(string.Empty));
        }

        [TestMethod]
        public void Ctor_Throws_ArgumentException_When_Name_Is_Whitespace()
        {
            // Arrange

            // Assert

            // Act
            Assert.ThrowsException<ArgumentException>(() => new GremlinPropertyAttribute(" "));
        }
    }
}