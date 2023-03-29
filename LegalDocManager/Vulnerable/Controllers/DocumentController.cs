using Dependencies.DataLayer;
using Dependencies.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Vulnerable.Models;

namespace Vulnerable.Controllers
{
    public class DocumentController : BaseController
    {
        public DocumentController(AppDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Required][FromForm] IFormFile file)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = ValidateUser();

            if (user == null)
                return BadRequest("This user is not authorized!");

            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);

            var document = new Document
            {
                FileExtension = fileExtension,
                FileName = fileName,
                ContentType = file.ContentType,
                User = user
            };

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            document.Content = stream.ToArray();

            context.Documents.Add(document);
            await context.SaveChangesAsync();

            return Ok("Document saved");
        }

        [HttpGet]
        public IActionResult GetFiles()
        {
            var user = ValidateUser();

            if (user == null)
                return BadRequest("This user is not authorized!");

            var documents = context.Documents
                .Where(x => x.UserId ==  user.Id)
                .Select(x => new GetDocumentDto
                {
                    Name = x.FileName,
                    File = new FileContentResult(x.Content, x.ContentType)
                })
                .ToList();

            return Ok(documents);
        }
    }
}
