namespace Improved.Models
{
    public class DocumentModel
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] Content { get; set; }
    }
}
