using System;
using Gremlin.Linq.Linq;
using Gremlin.Linq.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gremlin.Linq.Tests
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void TestEdgeSelection()
        {
            IGraphClient client = new TestGraphClient();
            var result = client
                .From<User>()
                .Where(a => a.FirstName == "name")
                .Where(a => a.Id == "id")
                .Where(a => a.Age == 3)
                .SelectOut<Login>("has")
                .BuildGremlinQuery();
            Console.WriteLine(result);
            Assert.AreEqual(
                "g.V().has('label','User').has('FirstName', 'name').has('Id', 'id').has('Age', 3).out('has').hasLabel('Login')",
                result);
        }

        [TestMethod]
        public void TestSimpleSelection()
        {
            IGraphClient client = new TestGraphClient();
            var query = client
                .Where("FirstName", "test")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('FirstName', 'test')", query);
        }

        [TestMethod]
        public void TestSimpleSelectionWithOut()
        {
            IGraphClient client = new TestGraphClient();
            var query = client
                .Where("FirstName", "test")
                .SelectOut<Login>("has")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('FirstName', 'test').out('has').hasLabel('Login')", query);
        }

        [TestMethod]
        public void TestAdd()
        {
            IGraphClient client = new TestGraphClient();
            var User = new User
            {
                FirstName = "name",
                Age = 3,
                Id = "id"
            };
            var query = client
                .Add(User)
                .BuildGremlinQuery();
            Assert.AreEqual(
                "g.addV('User').property('FirstName', 'name').property('Age', 3).property('Id', 'id')",
                query);
        }

        [TestMethod]
        public void TestAddOut()
        {
            IGraphClient client = new TestGraphClient();
            var Login = new Login
            {
                Id = "asdf"
            };
            var query = client
                .From<User>()
                .Where(a => a.Id == "123")
                .AddOut(Login, "has")
                .BuildGremlinQuery();
            Assert.AreEqual(
                "g.V().has('label','User').has('Id', '123').addE('has').to(g.addV('Login').property('Id', 'asdf')).inV()",
                query);
        }

        [TestMethod]
        public void TestWhereInSelector()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .WhereIn(a => a.FirstName, new[] {"test1", "test2"})
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('FirstName',within('test1','test2'))", q);
        }

        [TestMethod]
        public void TestWhereInSelectorWithOut()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .WhereIn(a => a.FirstName, new[] {"test1", "test2"})
                .Out<Login>()
                .BuildGremlinQuery();
            Assert.AreEqual(
                "g.V().has('label','User').has('FirstName',within('test1','test2')).out().has('label','Login')", q);
        }

        [TestMethod]
        public void TestAddConnectedEntities()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .Add(new User())
                .AddOut(new Login(), "has")
                .BuildGremlinQuery();
            Assert.AreEqual("g.addV('User').property('Age', 0).addE('has').to(g.addV('Login')).inV()", q);
        }
    }
}