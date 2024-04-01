using Domain;

namespace DAL;

public interface IGameRepository
{
    void SaveGame(Guid? id, GameState game);
    GameState? LoadGameState(Guid id);

    List<(Guid ID, DateTime LastEditedAt)> GetAllSaves();
    void UpdatePlayerNickname(Guid playerId, string newNickname); // Add this line

}