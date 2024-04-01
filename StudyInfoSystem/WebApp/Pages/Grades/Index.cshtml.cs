using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Grades
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
            Grade = new List<Grade>(); // Initialize Grade
        }

        public IList<Grade> Grade { get;set; } = default!;
        public double AverageGrade { get; set; }
        public object AverageGrades { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.StudentSubjects != null)
            {
                var studentSubjects = await _context.StudentSubjects
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Subject)
                    .Include(ss => ss.Grades)
                    .ToListAsync();

                foreach (var studentSubject in studentSubjects)
                {
                    if (studentSubject.Grades != null && studentSubject.Grades.Any())
                    {
                        // Calculate the average of all grades for this StudentSubject
                        var averageGrade = studentSubject.Grades.Average(g => g.Value);

                        // Assign this average value to the AverageGrade property of the StudentSubject
                        studentSubject.AverageGrade = averageGrade;
                    }
                }

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // After calculating the average grades, load the Grade list
                Grade = await _context.Grades
                    .Include(g => g.StudentSubject)
                    .ThenInclude(ss => ss.Student)
                    .Include(g => g.StudentSubject)
                    .ThenInclude(ss => ss.Subject)
                    .ToListAsync();
            }
        }
    }
}
