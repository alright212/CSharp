using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.BookAuthors
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

      public BookAuthor BookAuthor { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.BookAuthors == null)
            {
                return NotFound();
            }

            var bookauthor = await _context.BookAuthors.FirstOrDefaultAsync(m => m.Id == id);
            if (bookauthor == null)
            {
                return NotFound();
            }
            else 
            {
                BookAuthor = bookauthor;
            }
            return Page();
        }
    }
}
