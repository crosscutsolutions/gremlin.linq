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