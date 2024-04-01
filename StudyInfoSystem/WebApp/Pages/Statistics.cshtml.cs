using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class StatisticsModel : PageModel
{
    private readonly DAL.AppDbContext _context;

    public StatisticsModel(DAL.AppDbContext context)
    {
        _context = context;
    }

    public IList<SemesterStatistics> SemesterStatistics { get; set; } = new List<SemesterStatistics>();
    public IList<SubjectStatistics> SubjectStatistics { get; set; } = new List<SubjectStatistics>();

    public void OnGet()
    {
        SemesterStatistics = _context.StudentSubjects
            .Include(ss => ss.Semester)
            .Include(ss => ss.Subject)
            .GroupBy(ss => ss.Semester)
            .Select(g => new SemesterStatistics
            {
                Semester = g.Key,
                StudentCount = g.Select(ss => ss.StudentId).Distinct().Count(),
                TotalECTS = g.Sum(ss => ss.Subject.ECTS)
            })
            .ToList();

        SubjectStatistics = _context.StudentSubjects
            .Include(ss => ss.Subject)
            .GroupBy(ss => ss.Subject)
            .Select(g => new SubjectStatistics
            {
                Subject = g.Key,
                StudentCount = g.Select(ss => ss.StudentId).Distinct().Count(),
                TotalECTS = g.Where(ss => ss.EFinalGrade != EFinalGrade.Fail).Sum(ss => ss.Subject!.ECTS), // Only sum ECTS where EFinalGrade is not Fail
                AverageGrade = g.Average(ss => ss.AverageGrade)
            })
            .ToList();
    }
}

public class SemesterStatistics
{
    public Semester? Semester { get; set; }
    public int StudentCount { get; set; }
    public int TotalECTS { get; set; }
}
public class SubjectStatistics
{
    public Subject? Subject { get; set; }
    public int StudentCount { get; set; }
    public int TotalECTS { get; set; }
    public double AverageGrade { get; set; } 

}