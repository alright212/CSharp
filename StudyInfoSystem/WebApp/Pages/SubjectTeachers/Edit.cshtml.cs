using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.SubjectTeachers
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
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

            var subjectteacher =  await _context.SubjectTeachers.FirstOrDefaultAsync(m => m.Id == id);
            if (subjectteacher == null)
            {
                return NotFound();
            }
            SubjectTeacher = subjectteacher;
           ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Description");
           ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SubjectTeacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectTeacherExists(SubjectTeacher.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SubjectTeacherExists(int id)
        {
          return (_context.SubjectTeachers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
