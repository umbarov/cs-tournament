using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament;

public interface IPlayerService
{
    Results<Ok<Player>, BadRequest<string>> AddPlayer(Player player);
    Ok<List<Player>> GetPlayers();
    Results<Ok<Player>, NotFound> GetPlayer(Guid id);
    Results<Ok, NotFound> DeletePlayer(Guid id);
    bool PlayerExists(Guid id);
}