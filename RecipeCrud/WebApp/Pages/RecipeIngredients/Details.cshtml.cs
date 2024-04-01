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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

      public RecipeIngredient RecipeIngredient { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.RecipeIngredients == null)
            {
                return NotFound();
            }

            var recipeingredient = await _context.RecipeIngredients.FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipeingredient == null)
            {
                return NotFound();
            }
            else 
            {
                RecipeIngredient = recipeingredient;
            }
            return Page();
        }
    }
}
