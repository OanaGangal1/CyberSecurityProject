using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;

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

        /*
         using System;
using System.Data.SqlClient;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;

namespace CRUD_PDF
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect to the SQL Server database
            string connectionString = "Data Source=servername;Initial Catalog=databasename;User ID=username;Password=password;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create a new PDF document
                PdfDocument document = new PdfDocument();

                // Add a new page to the document
                PdfPage page = document.Pages.Add();

                // Draw some text on the page
                PdfGraphics graphics = page.Graphics;
                graphics.DrawString("Hello, world!", new PdfStandardFont(PdfFontFamily.Helvetica, 12), PdfBrushes.Black, new PointF(0, 0));

                // Save the document to a byte array
                byte[] pdfBytes;
                using (MemoryStream stream = new MemoryStream())
                {
                    document.Save(stream);
                    pdfBytes = stream.ToArray();
                }

                // Insert the byte array into the database as a new record
                SqlCommand insertCommand = new SqlCommand("INSERT INTO PdfFiles (FileName, FileContent) VALUES (@FileName, @FileContent)", connection);
                insertCommand.Parameters.AddWithValue("@FileName", "example.pdf");
                insertCommand.Parameters.AddWithValue("@FileContent", pdfBytes);
                insertCommand.ExecuteNonQuery();

                // Close the document
                document.Close(true);

                // Retrieve the record from the database
                SqlCommand selectCommand = new SqlCommand("SELECT FileContent FROM PdfFiles WHERE FileName = @FileName", connection);
                selectCommand.Parameters.AddWithValue("@FileName", "example.pdf");
                byte[] retrievedBytes = (byte[])selectCommand.ExecuteScalar();

                // Load the PDF file from the byte array
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(retrievedBytes);

                // Get the first page of the document
                PdfLoadedPage loadedPage = loadedDocument.Pages[0] as PdfLoadedPage;

                // Modify the page's content
                loadedPage.Graphics.DrawString("Modified text", new PdfStandardFont(PdfFontFamily.Helvetica, 12), PdfBrushes.Black, new PointF(0, 20));

                // Save the changes to the database as an update to the record
                byte[] modifiedBytes;
                using (MemoryStream stream = new MemoryStream())
                {
                    loadedDocument.Save(stream);
                    modifiedBytes = stream.ToArray();
                }
                SqlCommand updateCommand = new SqlCommand("UPDATE PdfFiles SET FileContent = @FileContent WHERE FileName = @FileName", connection);
                updateCommand.Parameters.AddWithValue("@FileName", "example.pdf");
                updateCommand.Parameters.AddWithValue("@FileContent", modifiedBytes);
                updateCommand.ExecuteNonQuery();

                // Close the document
                loadedDocument.Close(true);

                // Delete the record from the database
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM PdfFiles WHERE FileName = @FileName", connection);
                deleteCommand.Parameters.AddWithValue("@FileName", "example.pdf");
                deleteCommand.ExecuteNonQuery();
            }
        }
    }
}
         
         
         
         */


    }
}
