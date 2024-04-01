using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Domain;
using DAL;
using System.Linq;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context; // Your DbContext

    public IndexModel(ILogger<IndexModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IList<Recipe> Recipes { get; set; } = default!; // List of recipes to display

    public void OnGet(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            Recipes = _context.Recipes.ToList(); // Get all recipes if no search query
        }
        else
        {
            // Convert search query to lowercase
            var lowerCaseQuery = searchQuery.ToLower();

            // Filter recipes based on the search query (case-insensitive)
            Recipes = _context.Recipes
                .Where(r => r.Name.ToLower().Contains(lowerCaseQuery) 
                            || r.Description.ToLower().Contains(lowerCaseQuery))
                .ToList();
        }
    }
}