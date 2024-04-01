using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly DAL.AppDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(DAL.AppDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;

    }
    public int TotalBooks { get; set; }
    public int TotalAuthors { get; set; }
    public int TotalPublishers { get; set; }
    [BindProperty(SupportsGet = true)] public string? Search { get; set; } = "";

    [BindProperty(SupportsGet = true)] public bool InAuthor { get; set; }

    [BindProperty(SupportsGet = true)] public bool InTitle { get; set; }
    [BindProperty(SupportsGet = true)] public bool InPublisher { get; set; }
    [BindProperty(SupportsGet = true)] public bool InSummary { get; set; }
    [BindProperty(SupportsGet = true)] public bool InComment { get; set; }

    public IList<Book> Books { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {

        return Page();
    }

    public async Task OnGetAsync()
    {
        var query = _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)!
            .ThenInclude(ba => ba.Author)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            Search = Search.ToUpper();

            if (InTitle)
            {
                query = query.Where(b => b.Title.ToUpper().Contains(Search));
            }

            if (InPublisher)
            {
                query = query.Where(b => b.Publisher!.Name.ToUpper().Contains(Search));
            }

            if (InAuthor)
            {
                query = query.Where(b =>
                    b.Authors != null &&
                    b.Authors.Any(a => (a.Author!.FirstName + " " + a.Author.LastName).ToUpper()
                        .Contains(Search)));
            }

            if (InSummary)
            {
                query = query.Where(b => b.Description.ToUpper().Contains(Search));
            }
        }

        Books = await query.ToListAsync();
        
        TotalBooks = await _context.Books.CountAsync();
        TotalAuthors = await _context.Authors.CountAsync();
        TotalPublishers = await _context.Publishers.CountAsync();
    }
}
