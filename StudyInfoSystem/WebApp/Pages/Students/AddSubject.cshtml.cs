using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Students;

public class AddSubjectModel : PageModel
{
    private readonly DAL.AppDbContext _context;

    public AddSubjectModel(DAL.AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public int StudentId { get; set; }

    [BindProperty]
    public int SelectedSubjectId { get; set; }

    public SelectList SubjectSelectList { get; set; }

    public void OnGet(int studentId)
    {
        StudentId = studentId;
        SubjectSelectList = new SelectList(_context.Subjects, "Id", "Name");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var studentSubject = new StudentSubject
        {
            StudentId = StudentId,
            SubjectId = SelectedSubjectId
        };

        _context.StudentSubjects.Add(studentSubject);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}