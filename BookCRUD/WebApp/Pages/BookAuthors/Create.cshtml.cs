using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.BookAuthors
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
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName");
        ViewData["BookId"] = new SelectList(_context.Books, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public BookAuthor BookAuthor { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.BookAuthors == null || BookAuthor == null)
            {
                return Page();
            }

            _context.BookAuthors.Add(BookAuthor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
