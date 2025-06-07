using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament.Interfaces;

public interface IPlayerService
{
    Results<Ok<Player>, BadRequest<string>> AddPlayer(Player player);
    Ok<List<Player>> GetPlayers();
    Results<Ok<Player>, NotFound> GetPlayer(string username);
    Results<Ok, NotFound> DeletePlayer(string username);
    bool PlayerExists(string username);
}