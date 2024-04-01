using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.RecipeIngredients
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RecipeIngredient RecipeIngredient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.RecipeIngredients == null)
            {
                return NotFound();
            }

            var recipeingredient =  await _context.RecipeIngredients.FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipeingredient == null)
            {
                return NotFound();
            }
            RecipeIngredient = recipeingredient;
           ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name");
           ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RecipeIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeIngredientExists(RecipeIngredient.RecipeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RecipeIngredientExists(int id)
        {
          return (_context.RecipeIngredients?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }
    }
}
