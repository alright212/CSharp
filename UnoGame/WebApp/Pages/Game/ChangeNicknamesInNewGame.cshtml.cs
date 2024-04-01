using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameEngine;

namespace WebApp.Pages.Game;

public class ChangeNicknamesInNewGame : PageModel
{
    
    private readonly DAL.AppDbContext _context;
    private readonly IGameRepository _gameRepository;
    public GameEngine.GameEngine Engine { get; set; } = default!;
    
    public ChangeNicknamesInNewGame(AppDbContext context)
    {
        _context = context;
        _gameRepository = new GameRepositoryEF();
        Nicknames = new List<string>();
    }
    
    [BindProperty(SupportsGet = true)]
    public Guid GameId { get; set; }
    public void OnGet()
    {
        Engine = new GameEngine.GameEngine(_gameRepository);
        Engine.LoadGame(GameId);
        foreach (var player in Engine.GameState.Players)
        {
            if (player.Nickname != null) Nicknames.Add(player.Nickname);
        }
    }
    [BindProperty]
    public int PlayAsPlayerIndex { get; set; }

    [BindProperty]
    public List<string> Nicknames { get; set; }

    public async Task<IActionResult> OnPost()
    {
        Engine = new GameEngine.GameEngine(_gameRepository);
        Engine.LoadGame(GameId);

        for (int i = 0; i < Engine.GameState.Players.Count; i++)
        {
            Engine.GameState.Players[i].Nickname = Nicknames[i];
        }
            
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        Engine.SaveGame(Engine.GameState.Id);
        
        var playAsPlayerId = Engine.GameState.Players[PlayAsPlayerIndex].Id;
            
        return RedirectToPage("./GamePlay", routeValues:new
        {
            GameId=Engine.GameState.Id,
            PlayerId=playAsPlayerId
        });
    }
}