using CSTournament.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSTournament.Controllers;

[ApiController]
[Route("api/players")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    /// <summary>
    /// Adds a new player to the system.
    /// </summary>
    /// <param name="player">The player object to be added. It contains details such as username, name, and age.</param>
    /// <returns>
    /// Returns an Ok with the created player if the operation is successful,
    /// or a BadRequest with an error message if the input is invalid or the player could not be added.
    /// </returns>
    [HttpPost()]
    public Results<Ok<Player>, BadRequest<string>> AddPlayer([FromBody] Player player)
    {
        return _playerService.AddPlayer(player);
    }

    /// <summary>
    /// Retrieves a list of all registered players in the system.
    /// </summary>
    /// <returns>
    /// Returns an Ok result containing an enumerable collection of players.
    /// Each player object includes details such as username, name, and age.
    /// </returns>
    [HttpGet()]
    public Ok<IEnumerable<Player>> GetPlayers()
    {
        return _playerService.GetPlayers();
    }


    /// <summary>
    /// Retrieves the details of a specific player identified by their username.
    /// </summary>
    /// <param name="username">The unique identifier (username) of the player to retrieve.</param>
    /// <returns>
    /// Returns an Ok result with the player details if the player exists,
    /// or a NotFound result if no player with the specified username is found.
    /// </returns>
    [HttpGet("{username}")]
    public Results<Ok<Player>, NotFound> GetPlayer(string username)
    {
        return _playerService.GetPlayer(username);
    }

    /// <summary>
    /// Deletes a player from the system based on their username.
    /// </summary>
    /// <param name="username">The unique identifier (username) of the player to be deleted.</param>
    /// <returns>
    /// Returns an Ok result if the player was successfully deleted,
    /// or a NotFound result if no player with the specified username exists.
    /// </returns>
    [HttpDelete("{username}")]
    public Results<Ok, NotFound> DeletePlayer(string username)
    {
        return _playerService.DeletePlayer(username);
    }
}