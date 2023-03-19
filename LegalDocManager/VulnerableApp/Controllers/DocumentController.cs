using VulnerableApp.DataLayer;

namespace VulnerableApp.Controllers
{
    public class DocumentController : BaseController
    {
        public DocumentController(AppDbContext context) : base(context)
        {
        }
    }
}
