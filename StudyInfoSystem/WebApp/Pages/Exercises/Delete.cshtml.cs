using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Exercises
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Exercise Exercise { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Exercises == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }
            else 
            {
                Exercise = exercise;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Exercises == null)
            {
                return NotFound();
            }
            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise != null)
            {
                Exercise = exercise;
                _context.Exercises.Remove(Exercise);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
