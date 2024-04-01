using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.StudentSubjects
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
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

            var studentsubject = await _context.StudentSubjects.FirstOrDefaultAsync(m => m.Id == id);

            if (studentsubject == null)
            {
                return NotFound();
            }
            else 
            {
                StudentSubject = studentsubject;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.StudentSubjects == null)
            {
                return NotFound();
            }
            var studentsubject = await _context.StudentSubjects.FindAsync(id);

            if (studentsubject != null)
            {
                StudentSubject = studentsubject;
                _context.StudentSubjects.Remove(StudentSubject);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
