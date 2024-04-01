using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;


namespace WebApp.Pages.Teachers
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(DAL.AppDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync called.");

            ModelState.Remove("Teacher.Subjects");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid.");
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning(error.ErrorMessage);
                    }
                }

                return Page();
            }

            if (_context.Teachers == null)
            {
                _logger.LogWarning("_context.Teachers is null.");
                return Page();
            }

            if (Teacher == null)
            {
                _logger.LogWarning("Teacher is null.");
                return Page();
            }

            _context.Teachers.Add(Teacher);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Teacher added successfully. Redirecting to Index.");
            return RedirectToPage("./Index");
        }
    }
}
