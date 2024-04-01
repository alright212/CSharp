using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Database;

namespace WebApp.Pages.Game
{
    public class LoadGameModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private IGameRepository? _gameRepository;

        public LoadGameModel(DAL.AppDbContext context, IGameRepository? gameRepository)
        {
            _context = context;
            _gameRepository = gameRepository;
        }

        public IList<DbGame> DbGame { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Games != null)
            {
                // Fetching the games and including player information
                var gamesWithPlayers = await _context.Games
                    .Include(g => g.Players)
                    .OrderByDescending(g => g.UpdatedAt)
                    .ToListAsync();

                // Creating a new list to hold the updated games with correct nicknames
                DbGame = new List<DbGame>();

                foreach (var game in gamesWithPlayers)
                {
                    // Load each game using the game engine to ensure all business logic is applied
                    var engine = new GameEngine.GameEngine(_gameRepository);
                    engine.LoadGame(game.Id);

                    // Create a new DbGame object with updated player nicknames
                    var updatedGame = new DbGame
                    {
                        Id = game.Id,
                        CreatedAt = game.CreatedAt,
                        UpdatedAt = game.UpdatedAt,
                        Players = engine.GameState.Players.Select(p => new DbPlayer
                        {
                            Id = p.Id,
                            Nickname = p.Nickname,
                            PlayerType = p.Type
                        }).ToList()
                    };

                    DbGame.Add(updatedGame);
                }
            }
        }

    }
}