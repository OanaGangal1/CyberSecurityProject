namespace Dependencies.DataLayer.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] Content { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = new();
    }
}
