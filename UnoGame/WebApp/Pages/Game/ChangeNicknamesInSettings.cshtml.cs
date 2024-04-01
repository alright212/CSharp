using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Database;

namespace WebApp.Pages.Game
{
    public class ChangeNicknamesInSettingsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private readonly IGameRepository _gameRepository; // Change type from object to IGameRepository
        public GameEngine.GameEngine Engine { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public Guid GameId { get; set; }

        public ChangeNicknamesInSettingsModel(DAL.AppDbContext context, IGameRepository gameRepository)
        {
            _context = context;
            _gameRepository = gameRepository;
        }

        [BindProperty] public List<DbPlayer> Players { get; set; } = null!;

        public async Task OnGetAsync(Guid gameId)
        {
            // Load the game state using the game engine to ensure consistency
            Engine = new GameEngine.GameEngine(_gameRepository);
            Engine.LoadGame(gameId);

            // Convert the game state players to DbPlayers
            Players = Engine.GameState.Players.Select(p => new DbPlayer
            {
                Id = p.Id,
                Nickname = p.Nickname,
                // Populate other fields as necessary
            }).ToList();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Load the game state using the game engine to ensure consistency
            Engine = new GameEngine.GameEngine(_gameRepository);
            Engine.LoadGame(GameId);

            // Update the nicknames in the game engine's state
            for (int i = 0; i < Engine.GameState.Players.Count; i++)
            {
                Engine.GameState.Players[i].Nickname = Players[i].Nickname;
            }

            // Save the updated game state
            _gameRepository.SaveGame(GameId, Engine.GameState);

            return RedirectToPage("/Game/Settings");
        }
    }
}