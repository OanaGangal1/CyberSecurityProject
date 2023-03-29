using Microsoft.AspNetCore.Mvc;

namespace Vulnerable.Models
{
    public class GetDocumentDto
    {
        public string Name { get; set; }
        public FileContentResult File { get; set; }
    }
}
