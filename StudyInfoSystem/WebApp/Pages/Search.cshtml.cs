using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApp.Pages;

public class SearchModel : PageModel
{
    private readonly DAL.AppDbContext _context;

    public SearchModel(DAL.AppDbContext context)
    {
        _context = context;
    }

    public string SearchTerm { get; set; } = "";
    public bool StudentCheck { get; set; } = true;
    public bool TeacherCheck { get; set; } = true;
    public bool SubjectCheck { get; set; } = true;

    public IList<Student> Students { get; set; } = new List<Student>();
    public IList<Teacher> Teachers { get; set; } = new List<Teacher>();
    public IList<Subject> Subjects { get; set; } = new List<Subject>();

    public void OnGet(string searchTerm, bool studentCheck, bool teacherCheck, bool subjectCheck)
    {
        SearchTerm = searchTerm;
        StudentCheck = studentCheck;
        TeacherCheck = teacherCheck;
        SubjectCheck = subjectCheck;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            // SELECT 
            // s.*, 
            // ss.*, 
            // sub.*
            //     FROM 
            // Students s
            // LEFT JOIN 
            // StudentSubjects ss ON s.Id = ss.StudentId
            // LEFT JOIN 
            // Subjects sub ON ss.SubjectId = sub.Id;
            if (studentCheck)
            {
                Students = _context.Students.Include(s => s.StudentSubjects)!.ThenInclude(ss => ss.Subject).ToList();
            }

            if (teacherCheck)
            {
                Teachers = _context.Teachers.Include(t => t.SubjectTeachers)!.ThenInclude(st => st.Subject).ToList();
            }

            if (subjectCheck)
            {
                Subjects = _context.Subjects.ToList();
            }
        }
        else
        {
            var terms = searchTerm.Split(',');
            var inclusiveTerms = terms.Where(t => !t.StartsWith("!")).Select(t => t.Trim().ToLower());
            var exclusiveTerms = terms.Where(t => t.StartsWith("!")).Select(t => t.Trim().ToLower().Substring(1));
            /*SELECT *
                FROM Students s
            JOIN StudentSubjects ss ON s.Id = ss.StudentId
            JOIN Subjects sub ON ss.SubjectId = sub.Id
            WHERE (LOWER(s.Name) LIKE '%term%' OR LOWER(sub.Name) LIKE '%term%')
            AND NOT (LOWER(s.Name) LIKE '%!term%' OR LOWER(sub.Name) LIKE '%!term%');
            */
            if (studentCheck)

            {
                Students = _context.Students
                    .Include(s => s.StudentSubjects)!.ThenInclude(ss => ss.Subject)
                    .ToList()
                    .Where(s => inclusiveTerms.Any(it => s.Name.ToLower().Contains(it) ||
                                                         s.StudentSubjects!.Any(ss =>
                                                             ss.Subject!.Name.ToLower().Contains(it))) &&
                                !exclusiveTerms.Any(et => s.Name.ToLower().Contains(et) ||
                                                          s.StudentSubjects!.Any(ss =>
                                                              ss.Subject!.Name.ToLower().Contains(et))))
                    .ToList();
                /*SELECT 
                s.Id, 
                SUM(sub.ECTS) AS TotalECTS
                FROM 
                    Students s
                    JOIN 
                StudentSubjects ss ON s.Id = ss.StudentId
                JOIN 
                    Subjects sub ON ss.SubjectId = sub.Id
                WHERE 
                ss.EFinalGrade != 'Fail'  -- Replace 'Fail' with the actual value representing a fail grade in your database
                GROUP BY 
                s.Id;*/
                foreach (var student in Students)
                {
                    student.TotalECTS = student.StudentSubjects!.Where(ss => ss.EFinalGrade != EFinalGrade.Fail)
                        .Sum(ss => ss.Subject!.ECTS);
                }
            }
            /*SELECT *
                FROM Teachers t
            JOIN SubjectTeachers st ON t.Id = st.TeacherId
            JOIN Subjects sub ON st.SubjectId = sub.Id
            WHERE LOWER(sub.Name) LIKE '%term%' AND NOT LOWER(sub.Name) LIKE '%!term%';
            */
            if (teacherCheck)
            {
                Teachers = _context.Teachers
                    .Include(t => t.SubjectTeachers)!.ThenInclude(st => st.Subject)
                    .ToList()
                    .Where(t => inclusiveTerms.Any(it =>
                                    t.Name.ToLower().Contains(it) ||
                                    t.SubjectTeachers!.Any(st => st.Subject!.Name.ToLower().Contains(it))) &&
                                !exclusiveTerms.Any(et =>
                                    t.Name.ToLower().Contains(et) ||
                                    t.SubjectTeachers!.Any(st => st.Subject!.Name.ToLower().Contains(et))))
                    .ToList();

                foreach (var teacher in Teachers)
                {
                    teacher.Subjects = string.Join(", ", teacher.SubjectTeachers!.Select(st => st.Subject!.Name));
                }
            }
            /*SELECT *
                FROM Subjects
                WHERE LOWER(Name) LIKE '%term%' AND NOT LOWER(Name) LIKE '%!term%';
                */
            if (subjectCheck)
            {
                Subjects = _context.Subjects
                    .ToList()
                    .Where(s => inclusiveTerms.Any(it => s.Name.ToLower().Contains(it)) &&
                                !exclusiveTerms.Any(et => s.Name.ToLower().Contains(et)))
                    .ToList();
            }
        }
    }
}