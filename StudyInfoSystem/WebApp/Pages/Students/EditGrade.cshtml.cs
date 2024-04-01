using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Students;

public class EditGradeModel : PageModel
{
    private readonly DAL.AppDbContext _context;

    public EditGradeModel(DAL.AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Grade Grade { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Grade = await _context.Grades.FindAsync(id);

        if (Grade == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _context.Attach(Grade).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}