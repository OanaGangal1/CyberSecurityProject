using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Vulnerable.Models;
using Microsoft.Data.SqlClient;
using Vulnerable.Extensions;
using Dependencies.Entities.Vulnerable;

namespace Vulnerable.Controllers
{
    public class DocumentController : BaseController
    {
        public DocumentController(VulnerableDbContext context) : base(context)
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

            var user = ValidateUser();

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
        public IActionResult GetFile(string fileName)
        {
            using var connection = new SqlConnection(StartupExtensions.ConnectionString);
            var query = "SELECT ContentType, FileName, Id, Description FROM [v-legaldoc].dbo.Documents WHERE FileName LIKE '" + fileName + "%'";
            var cmd = connection.CreateCommand();
            cmd.CommandText = query;
            var fileResults = new List<FileModel>();

            try
            {
                connection.Open();
                var reader = cmd.ExecuteReader();
                if(reader != null)
                {
                    while (reader.Read())
                    {
                        fileResults.Add(new FileModel
                        {
                            Id = reader.GetInt32(2),
                            FileName = reader.GetString(1),
                            FileType = reader.GetString(0),
                            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                        });
                    }
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
            return Ok("File was deleted");
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

                return File(content, contentType);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return BadRequest("Could not download file");
            }
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
