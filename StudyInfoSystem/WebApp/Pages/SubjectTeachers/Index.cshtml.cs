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
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<SubjectTeacher> SubjectTeacher { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.SubjectTeachers != null)
            {
                SubjectTeacher = await _context.SubjectTeachers
                .Include(s => s.Subject)
                .Include(s => s.Teacher).ToListAsync();
            }
        }
    }
}
