using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.RecipeIngredients
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public RecipeIngredient RecipeIngredient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? recipeId, int? ingredientId)
        {
            if (recipeId == null || ingredientId == null || _context.RecipeIngredients == null)
            {
                return NotFound();
            }

            RecipeIngredient = await _context.RecipeIngredients
                .FirstOrDefaultAsync(m => m.RecipeId == recipeId && m.IngredientId == ingredientId);

            if (RecipeIngredient == null)
            {
                return NotFound();
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? recipeId, int? ingredientId)
        {
            if (recipeId == null || ingredientId == null || _context.RecipeIngredients == null)
            {
                return NotFound();
            }

            var recipeingredient = await _context.RecipeIngredients
                .FirstOrDefaultAsync(m => m.RecipeId == recipeId && m.IngredientId == ingredientId);

            if (recipeingredient != null)
            {
                _context.RecipeIngredients.Remove(recipeingredient);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }
}
