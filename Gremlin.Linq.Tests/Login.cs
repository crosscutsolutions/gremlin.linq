namespace Gremlin.Linq.Tests
{
    public class Login : Vertex
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProviderName { get; set; }
        public string ProviderSubjectId { get; set; }
    }
}