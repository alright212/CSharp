using System.Text.Json;
using Domain;
using Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GameRepositoryEF : IGameRepository
{
    private static string connectionString =
        "DataSource=<%temppath>Uno\\app.db".Replace("<%temppath>", Path.GetTempPath());

    private static readonly DbContextOptions<AppDbContext> ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(connectionString)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .Options;


    private readonly AppDbContext _ctx = new(ContextOptions);


    public void SaveGame(Guid? id, GameState gameState)
    {
        var game = _ctx.Games.FirstOrDefault(g =>
            g.Id == gameState.Id); //SELECT TOP 1 * FROM Games WHERE Id = @gameStateId
        if (game == null)
        {
            game =
                new
                    DbGame //INSERT INTO Games (Id, State, CreatedAt, UpdatedAt) VALUES (@Id, @State, @CreatedAt, @UpdatedAt);
                    {
                        Id = gameState.Id,
                        State = JsonSerializer.Serialize(game),
                        Players = gameState.Players.Select(p => new DbPlayer()
                        {
                            Id = p.Id,
                            Nickname = p.Nickname,
                            PlayerType = p.Type
                        }).ToList(),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
            game.State = JsonSerializer.Serialize(gameState);
            _ctx.Games.Add(game);
        }
        else //UPDATE Games
            // SET UpdatedAt = @UpdatedAt, State = @State
            // WHERE Id = @Id;
        {
            game.UpdatedAt = DateTime.Now;
            game.State = JsonSerializer.Serialize(gameState);
        }

        var changeCount = _ctx.SaveChanges();
    }

    public GameState LoadGameState(Guid id)
    {
        var game = _ctx.Games.FirstOrDefault(g => g.Id == id); // SELECT TOP 1 * FROM Games WHERE Id = @id;
        if (game == null)
        {
            // Handle the case where the game doesn't exist, for example:
            throw new KeyNotFoundException($"No game found with ID {id}");
        }

        return JsonSerializer.Deserialize<GameState>(game.State) ??
               throw new InvalidOperationException("Failed to deserialize game state.");
    }

    public void UpdatePlayerNickname(Guid playerId, string newNickname)
    {
        var player = _ctx.Players.Find(playerId); //SELECT * FROM Players WHERE Id = @playerId;
        if (player != null) //UPDATE Players
            // SET Nickname = @newNickname
            // WHERE Id = @playerId;
        {
            player.Nickname = newNickname;
            _ctx.SaveChanges();
        }
    }

    public List<(Guid ID, DateTime LastEditedAt)> GetAllSaves()
    {
        return _ctx.Games//SELECT Id, UpdatedAt FROM Games ORDER BY UpdatedAt DESC;
            .OrderByDescending(g => g.UpdatedAt)
            .ToList()
            .Select(g => (g.Id, g.UpdatedAt))
            .ToList();
    }
}