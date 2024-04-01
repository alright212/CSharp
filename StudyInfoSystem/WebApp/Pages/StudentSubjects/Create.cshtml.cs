using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.StudentSubjects
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
        ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
        ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public StudentSubject StudentSubject { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.StudentSubjects == null || StudentSubject == null)
            {
                return Page();
            }

            // If there are any grades, calculate the average grade
            if (StudentSubject.Grades != null && StudentSubject.Grades.Any())
            {
                StudentSubject.AverageGrade = StudentSubject.Grades.Average(g => g.Value);
            }

            _context.StudentSubjects.Add(StudentSubject);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
