using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament;

public class PlayerService : IPlayerService
{
    private readonly Dictionary<Guid, Player> _players = new();

    public Results<Ok<Player>, BadRequest<string>> AddPlayer(Player player)
    {
        return !_players.TryAdd(player.Id, player)
            ? TypedResults.BadRequest($"Player {player.Id} already exists.")
            : TypedResults.Ok(player);
    }

    public Ok<List<Player>> GetPlayers()
    {
        return TypedResults.Ok(_players.Values.ToList());
    }

    public Results<Ok<Player>, NotFound> GetPlayer(Guid id)
    {
        return !_players.TryGetValue(id, out var player)
            ? TypedResults.NotFound()
            : TypedResults.Ok(player);
    }

    public Results<Ok, NotFound> DeletePlayer(Guid id)
    {
        return _players.Remove(id)
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }

    public bool PlayerExists(Guid id)
    {
        return _players.ContainsKey(id);
    }
}