namespace Dependencies.DataLayer.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public List<Document> Documents { get; set; } = new();
    }
}
