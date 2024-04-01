using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.SubjectTeachers
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
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public SubjectTeacher SubjectTeacher { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.SubjectTeachers == null || SubjectTeacher == null)
            {
                return Page();
            }

            _context.SubjectTeachers.Add(SubjectTeacher);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
