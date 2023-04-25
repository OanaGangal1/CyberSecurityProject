namespace Dependencies.Entities.Improved
{
    public class Document
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string? Description { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
