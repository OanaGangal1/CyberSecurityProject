using Dependencies.DataLayer;
using Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Vulnerable.Models;
using Microsoft.Data.SqlClient;
using Vulnerable.Extensions;
using System.Xml.Linq;

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
            if (!ModelState.IsValid)
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
        public IActionResult GetFile(string fileName)
        {
            using var connection = new SqlConnection(StartupExtensions.ConnectionString);
            var query = "SELECT ContentType, FileName, Id FROM [v-legaldoc].dbo.Documents WHERE FileName LIKE '" + fileName + "%'";
            var cmd = connection.CreateCommand();
            cmd.CommandText = query;
            var fileResults = new List<FileModel>();

            try
            {
                connection.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    fileResults.Add(new FileModel
                    {
                        Id = reader.GetInt32(2),
                        FileName = reader.GetString(1),
                        FileType = reader.GetString(0)
                    });
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok(fileResults);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await context.Documents.FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
                return NotFound("File not found");

            context.Documents.Remove(file);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public IActionResult Download(int id)
        {
            using var connection = new SqlConnection(StartupExtensions.ConnectionString);
            var query = "SELECT Content, ContentType FROM [v-legaldoc].dbo.Documents WHERE Id = " + id;
            var cmd = connection.CreateCommand();
            cmd.CommandText = query;

            var contentType = string.Empty;
            var content = Array.Empty<byte>();

            try
            {
                connection.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    content = (byte[])reader[0];
                    contentType = reader.GetString(1);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return File(content, contentType);
        }

        private List<FileModel>? GetFiles()
        {
            var user = ValidateUser();

            if (user == null)
                return null;

            var documents = context.Documents
                .Include(x => x.User)
                .Where(x => x.UserId == user.Id)
                .Select(x => new FileModel
                {
                    FileName = x.FileName,
                    File = new FileContentResult(x.Content, x.ContentType)
                })
                .ToList();

            return documents;
        }
    }
}
