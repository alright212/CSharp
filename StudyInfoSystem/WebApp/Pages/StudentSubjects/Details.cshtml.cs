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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

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
    }
}
