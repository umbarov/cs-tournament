using CSTournament.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSTournament.Controllers;

[ApiController]
[Route("api/tournaments")]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    
    public TournamentsController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    /// <summary>
    /// Creates a new tournament based on the provided request data.
    /// </summary>
    /// <param name="req">An instance of <see cref="TournamentCreateRequest"/> containing the details for the tournament to be created.</param>
    /// <returns>
    /// A result object that can contain one of the following:
    /// - <see cref="Ok{T}"/> containing the created tournament.
    /// - <see cref="BadRequest{T}"/> with an error message if the request data is invalid.
    /// - <see cref="NotFound{T}"/> with an error message if a parent tournament specified in the request was not found.
    /// </returns>
    [HttpPost()]
    public Results<Ok<Tournament>, BadRequest<string>, NotFound<string>> CreateTournament(TournamentCreateRequest req)
    {
        return _tournamentService.CreateTournament(req);
    }


    /// <summary>
    /// Retrieves a collection of all tournaments.
    /// </summary>
    /// <returns>
    /// A result object containing an <see cref="Ok{T}"/> with a collection of tournaments.
    /// </returns>
    [HttpGet()]
    public Ok<IEnumerable<Tournament>> GetTournaments()
    {
        return _tournamentService.GetTournaments();
    }

    /// <summary>
    /// Retrieves the details of a specific tournament identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the tournament to retrieve.</param>
    /// <returns>
    /// A result object that can contain one of the following:
    /// - <see cref="Ok{T}"/> containing the tournament details.
    /// - <see cref="NotFound"/> if no tournament is found with the provided ID.
    /// </returns>
    [HttpGet("{id}")]
    public Results<Ok<Tournament>, NotFound> GetTournamentDetails(string id)
    {
        return _tournamentService.GetTournamentDetails(id);
    }

    /// <summary>
    /// Deletes an existing tournament specified by the ID.
    /// </summary>
    /// <param name="id">The unique identifier of the tournament to be deleted.</param>
    /// <returns>
    /// A result object that can contain one of the following:
    /// - <see cref="Ok{T}"/> containing a confirmation message upon successful deletion.
    /// - <see cref="NotFound{T}"/> with an error message if the tournament with the specified ID was not found.
    /// </returns>
    [HttpDelete("{id}")]
    public Results<Ok<string>, NotFound<string>> DeleteTournament(string id)
    {
        return _tournamentService.DeleteTournament(id);
    }

    /// <summary>
    /// Registers a player to a specified tournament.
    /// </summary>
    /// <param name="id">The unique identifier of the tournament where the player should be registered.</param>
    /// <param name="username">The username of the player to be registered.</param>
    /// <returns>
    /// A result object that can contain one of the following:
    /// - <see cref="Ok"/> if the player was successfully registered.
    /// - <see cref="BadRequest{T}"/> containing an error message if the registration failed due to invalid data or constraints.
    /// - <see cref="NotFound{T}"/> containing an error message if the specified tournament was not found.
    /// </returns>
    [HttpPost("{id}/register")]
    public Results<Ok, BadRequest<string>, NotFound<string>> RegisterPlayer(string id, string username)
    {
        return _tournamentService.RegisterPlayer(id, username);
    }
}