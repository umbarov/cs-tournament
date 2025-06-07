using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament;

interface ITournamentService
{
    Results<Ok<Tournament>, BadRequest<string>, NotFound<string>> CreateTournament(TournamentCreateRequest req);
    Ok<List<Tournament>> GetTournaments();
    Results<Ok<Tournament>, NotFound> GetTournamentDetails(Guid id);
    Results<Ok<string>, NotFound<string>> DeleteTournament(Guid id);
    Results<Ok, BadRequest<string>, NotFound<string>> RegisterPlayer(Guid tournamentId, Guid playerId);
}