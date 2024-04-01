using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Grades
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["StudentSubjectId"] = new SelectList(_context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                .Select(ss => new
                {
                    Id = ss.Id,
                    Name = $"{ss.Student.Name} - {ss.Subject.Name}"
                }), "Id", "Name");
            Grade = new Grade(); // Initialize the Grade object
            return Page();
        }

        [BindProperty]
        public Grade Grade { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["StudentSubjectId"] = new SelectList(_context.StudentSubjects, "Id", "Id"); // Re-populate the SelectList if the model state is invalid
                return Page();
            }

            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
