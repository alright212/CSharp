using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain.Database;
using GameEngine;

namespace WebApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private readonly IGameRepository _gameRepository;
        [BindProperty, Range(0, 10)] public int HumanPlayerCount { get; set; }

        [BindProperty, Range(0, 10)] public int AiPlayerCount { get; set; }

        public global::GameEngine.GameEngine Engine = default!;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
            _gameRepository = new GameRepositoryEF();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty,
         Range(0, 10)]
        public int PlayerCount { get; set; }

        [BindProperty,
         Range(0, 3)]
        public int PlayerTypes { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPost()
        {
            Engine = new GameEngine.GameEngine(_gameRepository);

            // Custom Validation: Ensure there are enough players for a game
            if (HumanPlayerCount < 1)
            {
                ModelState.AddModelError("", "There must be at least one human player.");
            }
            else if (HumanPlayerCount == 1 && AiPlayerCount < 1)
            {
                ModelState.AddModelError("",
                    "There must be at least one AI player when there is only one human player.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Engine.SetCustomPlayerTypes(HumanPlayerCount, AiPlayerCount);

            Engine.SaveGame(Engine.GameState.Id);
            return RedirectToPage("./ChangeNicknamesInNewGame", new { GameId = Engine.GameState.Id });
        }
    }
}