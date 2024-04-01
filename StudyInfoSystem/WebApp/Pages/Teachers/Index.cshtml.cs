using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Teacher> Teacher { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Teachers != null)
            {
                Teacher = await _context.Teachers.ToListAsync();
            }
        }
    }
}
