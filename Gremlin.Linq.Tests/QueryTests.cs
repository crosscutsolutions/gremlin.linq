using System.Security.Cryptography.X509Certificates;

namespace Gremlin.Linq.Tests
{
    using System;
    using Entities;
    using Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                "g.V().has('label','User').has('FirstName', 'name').has('Id', 'id').has('Age', 3).out('has').hasLabel('Login')", result);
        }

        [TestMethod]
        public void TestSimpleSelection()
        {
            IGraphClient client = new TestGraphClient();
            string query = client
                .Where("FirstName", "test")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('FirstName', 'test')", query);
        }

        [TestMethod]
        public void TestSimpleSelectionWithOut()
        {
            IGraphClient client = new TestGraphClient();
            string query = client
                .Where("FirstName", "test")
                .SelectOut<Login>("has")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('FirstName', 'test').out('has').hasLabel('Login')", query);
        }

        [TestMethod]
        public void TestAdd()
        {
            IGraphClient client = new TestGraphClient();
            User User = new User
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
            var Login = new Login()
            {
                Id = "asdf"
            };
            var query = client
                .From<User>()
                .Where(a => a.Id == "123")
                .AddOut(Login, "has")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('Id', '123').addE('has').to(g.addV('Login').property('Id', 'asdf')).inV()",query);
        }

        [TestMethod]
        public void TestCountWithFilter()
        {
            IGraphClient client = new TestGraphClient();
            var query = client
                .From<User>()
                .Where(a => a.FirstName == "asdf")
                .Count()
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('FirstName', 'asdf').count()",query);
        }

        [TestMethod]
        public void TestCountWithoutFilter()
        {
            IGraphClient client = new TestGraphClient();
            var query = client
                .From<User>()
                .Count()
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').count()", query);
        }

        [TestMethod]
        public void TestWhereInSelector()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .WhereIn(a => a.FirstName, new string[] {"test1", "test2"})
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('FirstName',within('test1','test2'))", q);
        }

        [TestMethod]
        public void TestWhereInSelectorWithOut()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .WhereIn(a => a.FirstName, new string[] {"test1", "test2"})
                .Out<Login>()
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('FirstName',within('test1','test2')).out().has('label','Login')", q);
        }

        [TestMethod]
        public void TestAddConnectedEntities()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .Add(new User())
                .AddOut(new Login(),"has")
                .BuildGremlinQuery();
            Assert.AreEqual("g.addV('User').property('Age', 0).addE('has').to(g.addV('Login')).inV()", q);
        }

        [TestMethod]
        public void TestSimpleWhere()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .Where(a => a.FirstName =="kalle")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('FirstName', 'kalle')",q);
        }

        [TestMethod]
        public void TestWhereWithEqual()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .Where(a => a.FirstName.Equals("kalle"))
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('FirstName', 'kalle')",q);
        }

        [TestMethod]
        public void TestWhereWithMultipleConditions()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .Where(a => a.FirstName.Equals("kalle") && a.LastName=="Sven" && a.Age>3)
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('FirstName', 'kalle').has('LastName', 'Sven').has('Age', gt(3))",q);
        }

        [TestMethod]
        public void TestWhereWithOrStatement()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .Where(a => a.FirstName.Equals("kalle") && (a.LastName == "Sven" || a.Age > 3))
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('FirstName', 'kalle').has('LastName', 'Sven').has('Age', gt(3))", q);
        }

        [TestMethod]
        public void TestCorrectCaseOnBoolProperties()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<UserWithBool>()
                .Where(a => a.Active == true)
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','UserWithBool').has('Active', true)", q);
        }

        [TestMethod]
        public void TestJsonPropertyNameOverrideWhere()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<User>()
                .Where(x => x.TestJsonPropertyName == "a")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','User').has('NameOverride', 'a')", q);
        }

        [TestMethod]
        public void TestJsonPropertyNameOverrideSet()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .Add(new User
                {
                    Age = 10,
                    FirstName = "name",
                    TestJsonPropertyName = "a"
                })
                .BuildGremlinQuery();
            Assert.AreEqual("g.addV('User').property('FirstName', 'name').property('Age', 10).property('NameOverride', 'a')", q);
        }

        [TestMethod]
        public void TestCustomLabelForUser()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<UserWithCustomLabel>()
                .Where(a => a.Name == "name")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','CustomUser').has('Name', 'name')", q);
        }

        [TestMethod]
        public void TestCustomJsonNetLabelForUser()
        {
            IGraphClient client = new TestGraphClient();
            var q = client
                .From<UserWithCustomLabelUsingJsonNet>()
                .Where(a => a.Name == "name")
                .BuildGremlinQuery();
            Assert.AreEqual("g.V().has('label','JsonNetUser').has('Name', 'name')", q);
        }
    }
}