using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class WinPageModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Winner { get; set; } = null!;

        public void OnGet()
        {
        }
    }
}