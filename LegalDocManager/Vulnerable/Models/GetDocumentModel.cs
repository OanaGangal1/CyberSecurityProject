using Microsoft.AspNetCore.Mvc;

namespace Vulnerable.Models
{
    public class GetDocumentModel
    {
        public List<FileModel> Files { get; set; }
    }

    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string? Description { get; set; }
        public string FileType { get; set; }
        public FileContentResult File { get; set; }
    }
}
