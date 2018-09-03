namespace Gremlin.Linq.Entities
{
    using System.Collections.Generic;

    public class User : Vertex
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
        public string SubjectId { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
    }

    public class UserWithBool : Vertex
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}