using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Improved.Models;
using Microsoft.AspNetCore.Authorization;
using Dependencies.Entities.Improved;
using Microsoft.AspNetCore.Identity;

namespace Improved.Controllers
{
    [Authorize]
    public class DocumentController : BaseController
    {
        public DocumentController(ImprovedDbContext context, UserManager<User> userManager) : base(context, userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Required][FromForm] AddFileModel addFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await ValidateUserAsync();

            if (user == null)
                return BadRequest("This user is not authorized!");

            var fileName = Path.GetFileName(addFile.File.FileName);
            var fileExtension = Path.GetExtension(addFile.File.FileName);

            var document = new Document
            {
                FileExtension = fileExtension,
                FileName = fileName,
                ContentType = addFile.File.ContentType,
                Description = addFile.Description,
                User = user
            };

            using var stream = new MemoryStream();
            await addFile.File.CopyToAsync(stream);
            document.Content = stream.ToArray();

            context.Documents.Add(document);
            await context.SaveChangesAsync();

            return Ok("Document saved");
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var user = await ValidateUserAsync();
            if(user == null)
                return BadRequest("You have no authorization!");

            var files = context.Documents
                .Where(x => x.UserId == user.Id && x.FileName.Contains(fileName))
                .Select(x => new FileModel
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    FileType = x.ContentType,
                    Description = x.Description
                })
                .ToList();

            return Ok(files);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            var user = await ValidateUserAsync();
            if (user == null)
                return BadRequest("You have no authorization!");

            var file = await context.Documents.FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);
            if (file == null)
                return NotFound("File not found");

            context.Documents.Remove(file);
            await context.SaveChangesAsync();
            return Ok("File was deleted");
        }

        [HttpGet]
        public async Task<IActionResult> Download(Guid id)
        {
            var user = await ValidateUserAsync();
            if (user == null)
                return BadRequest("You have no authorization!");

            var file = context.Documents.FirstOrDefault(x => x.Id == id && x.UserId == user.Id);
            if (file == null)
                return NotFound("File was not found!");

            return File(file.Content, file.ContentType);
        }

        private List<FileModel>? GetFiles()
        {
            //var user = ValidateUser();

            //if (user == null)
            //    return null;

            var documents = context.Documents
                //.Include(x => x.User)
                //.Where(x => x.UserId == user.Id)
                .Select(x => new FileModel
                {
                    FileName = x.FileName,
                    Description = x.Description,
                    FileType = x.ContentType,
                    File = new FileContentResult(x.Content, x.ContentType)
                })
                .ToList();

            return documents;
        }
    }
}
