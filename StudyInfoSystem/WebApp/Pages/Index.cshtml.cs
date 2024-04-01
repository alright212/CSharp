using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public int SubjectCount { get; set; }
        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }

        public void OnGet()
        {
            SubjectCount = _context.Subjects.Count();
            StudentCount = _context.Students.Count();
            TeacherCount = _context.Teachers.Count();
        }
    }
}