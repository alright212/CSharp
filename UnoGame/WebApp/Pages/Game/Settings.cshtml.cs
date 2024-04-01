using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Database;

namespace WebApp.Pages.Game
{
    public class SettingsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private readonly IGameRepository _gameRepository; // Ensure this is injected or initialized properly

        public SettingsModel(DAL.AppDbContext context, IGameRepository gameRepository)
        {
            _context = context;
            _gameRepository = gameRepository; // Initialize through dependency injection if not already
        }

        [BindProperty] public List<DbGame> Games { get; set; } = null!;

        public async Task OnGetAsync()
        {
            // Fetch games without players. We will load players through the GameEngine.
            var gameIds = await _context.Games
                .Select(g => g.Id)
                .ToListAsync();

            Games = new List<DbGame>();
            foreach (var gameId in gameIds)
            {
                var engine = new GameEngine.GameEngine(_gameRepository);
                engine.LoadGame(gameId);

                var dbGame = new DbGame
                {
                    Id = gameId,
                    Players = engine.GameState.Players.Select(p => new DbPlayer
                    {
                        Id = p.Id,
                        Nickname = p.Nickname,
                        PlayerType = p.Type
                    }).ToList()
                };

                Games.Add(dbGame);
            }
        }
    }
}