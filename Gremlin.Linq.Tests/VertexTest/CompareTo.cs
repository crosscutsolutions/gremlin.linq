using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests.VertexTest
{
    [TestClass]
    public class VertexTest
    {
        [TestMethod]
        public void CompareTo_Returns_NonZero_When_Id_Properties_Differ()
        {
            // Arrange
            var first = new Vertex {Id = "first unit test id"};
            var second = new Vertex {Id = "second unit test id"};

            // Assert
            var result = first.CompareTo(second);

            // Act
            Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_Returns_NonZero_When_Id_Properties_Differ_By_Case()
        {
            // Arrange
            var first = new Vertex {Id = "UNIT TEST ID"};
            var second = new Vertex {Id = "unit test id"};

            // Assert
            var result = first.CompareTo(second);

            // Act
            Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_Returns_Zero_When_Id_Properties_Match()
        {
            // Arrange
            var first = new Vertex {Id = "unit test id"};
            var second = new Vertex {Id = "unit test id"};

            // Assert
            var result = first.CompareTo(second);

            // Act
            Assert.AreEqual(0, result);
        }
    }
}