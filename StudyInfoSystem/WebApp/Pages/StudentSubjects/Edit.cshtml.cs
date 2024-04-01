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

namespace WebApp.Pages.StudentSubjects
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentSubject StudentSubject { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.StudentSubjects == null)
            {
                return NotFound();
            }

            var studentsubject =  await _context.StudentSubjects.FirstOrDefaultAsync(m => m.Id == id);
            if (studentsubject == null)
            {
                return NotFound();
            }
            StudentSubject = studentsubject;
           ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Id");
           ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
           ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Description");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // If there are any grades, calculate the average grade
            if (StudentSubject.Grades != null && StudentSubject.Grades.Any())
            {
                StudentSubject.AverageGrade = StudentSubject.Grades.Average(g => g.Value);
            }

            _context.Attach(StudentSubject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentSubjectExists(StudentSubject.Id))
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

        private bool StudentSubjectExists(int id)
        {
          return (_context.StudentSubjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
