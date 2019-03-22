using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests
{
    [TestClass]
    public class GremlinLabelAttributeTest
    {
        [TestMethod]
        public void Ctor_Throws_ArgumentException_When_Label_Is_Null()
        {
            // Arrange

            // Assert

            // Act
            Assert.ThrowsException<ArgumentException>(() => new GremlinLabelAttribute(null));
        }

        [TestMethod]
        public void Ctor_Throws_ArgumentException_When_Label_Is_Empty()
        {
            // Arrange

            // Assert

            // Act
            Assert.ThrowsException<ArgumentException>(() => new GremlinLabelAttribute(string.Empty));
        }

        [TestMethod]
        public void Ctor_Throws_ArgumentException_When_Label_Is_Whitespace()
        {
            // Arrange

            // Assert

            // Act
            Assert.ThrowsException<ArgumentException>(() => new GremlinLabelAttribute(" "));
        }
    }
}