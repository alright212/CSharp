using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.SubjectTeachers
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SubjectTeacher SubjectTeacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SubjectTeachers == null)
            {
                return NotFound();
            }

            var subjectteacher = await _context.SubjectTeachers.FirstOrDefaultAsync(m => m.Id == id);

            if (subjectteacher == null)
            {
                return NotFound();
            }
            else 
            {
                SubjectTeacher = subjectteacher;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SubjectTeachers == null)
            {
                return NotFound();
            }
            var subjectteacher = await _context.SubjectTeachers.FindAsync(id);

            if (subjectteacher != null)
            {
                SubjectTeacher = subjectteacher;
                _context.SubjectTeachers.Remove(SubjectTeacher);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
