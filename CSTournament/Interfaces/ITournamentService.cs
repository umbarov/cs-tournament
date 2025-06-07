using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament.Interfaces;

public interface ITournamentService
{
    Results<Ok<Tournament>, BadRequest<string>, NotFound<string>> CreateTournament(TournamentCreateRequest req);
    Ok<IEnumerable<Tournament>> GetTournaments();
    Results<Ok<Tournament>, NotFound> GetTournamentDetails(string id);
    Results<Ok<string>, NotFound<string>> DeleteTournament(string id);
    Results<Ok, BadRequest<string>, NotFound<string>> RegisterPlayer(string tournamentId, string username);
}