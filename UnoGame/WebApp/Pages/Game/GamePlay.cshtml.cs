using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameEngine;

namespace WebApp.Pages.Game;

public class GamePlay : PageModel
{
    private readonly DAL.AppDbContext _context;
    private readonly IGameRepository _gameRepository;
    public GameEngine.GameEngine Engine { get; set; } = default!;
    public bool AskForColor { get; set; } = false;

    public GamePlay(AppDbContext context, IGameRepository gameRepository)
    {
        _context = context;
        _gameRepository = gameRepository;
    }

    private void PlayUntilHuman()
    {
        while (Engine.GetActivePlayer().Type != EPlayerType.Human)
        {
            Engine.AiPlayRandomCard();
            Engine.SaveGame(GameId);
        }
        Engine.DrawUntilPlayableCard();
        Engine.SaveGame(GameId);
    }
    


    [BindProperty(SupportsGet = true)]
    public Guid GameId { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public Guid PlayerId { get; set; }
    
    public void OnGet()
    {
        Engine = new GameEngine.GameEngine(_gameRepository);
        Engine.LoadGame(GameId);
        if (!AskForColor)
        {
            PlayUntilHuman();
        }
    }
    
    public async Task<IActionResult> OnPostAsync(int? card)
    {
        Engine = new GameEngine.GameEngine(_gameRepository);
        Engine.LoadGame(GameId);
        var activePlayer = Engine.GetActivePlayer();
        Console.WriteLine(card);
        if (card is < -1 and > -6)
        {
            Console.WriteLine(card);
            switch (card)
            {
                case -3:
                    Engine.SetTopCardColor("l");
                    break;
                case -4:
                    Engine.SetTopCardColor("g");
                    break;
                case -5:
                    Engine.SetTopCardColor("y");
                    break;
                case -6:
                    Engine.SetTopCardColor("r");
                    break;
            }
            AskForColor = false;
        }
        if ((activePlayer.Id == PlayerId && activePlayer.Type == EPlayerType.Human))
        {
            //Skipped turn
            if (card == -1)
            {
                Console.WriteLine("skipped");
                Engine.IncreasePlayerIndex();
                Engine.SaveGame(GameId);
                return Page();
            }
            else if (card is > -1)
            {
                var res = Engine.HumanPlayCard(Engine.GameState.CurrentPlayerIndex, card.Value);
                if (res == "askForColor")
                {
                    AskForColor = true;
                    Engine.SaveGame(GameId);
                    return Page();
                }
               
                if (res == "won")
                {
                    Engine.SaveGame(GameId); // Save the game state when a player wins
                    _context.Games.Remove(_context.Games.First(g => g.Id == GameId));
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Game/WinPage", new { Winner = activePlayer.Nickname });
                }
            }
        }
        Engine.SaveGame(GameId);
        PlayUntilHuman();
        return Page();
    }
}