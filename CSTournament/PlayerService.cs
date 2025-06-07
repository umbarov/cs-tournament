using CSTournament.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament;

public class PlayerService : IPlayerService
{
    private readonly Dictionary<string, Player> _players = new(StringComparer.OrdinalIgnoreCase);

    public Results<Ok<Player>, BadRequest<string>> AddPlayer(Player player)
    {
        return !_players.TryAdd(player.Username, player)
            ? TypedResults.BadRequest($"Player {player.Username} already exists.")
            : TypedResults.Ok(player);
    }

    public Ok<List<Player>> GetPlayers()
    {
        return TypedResults.Ok(_players.Values.ToList());
    }

    public Results<Ok<Player>, NotFound> GetPlayer(string username)
    {
        return !_players.TryGetValue(username, out var player)
            ? TypedResults.NotFound()
            : TypedResults.Ok(player);
    }

    public Results<Ok, NotFound> DeletePlayer(string username)
    {
        return _players.Remove(username)
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }

    public bool PlayerExists(string username)
    {
        return _players.ContainsKey(username);
    }
}