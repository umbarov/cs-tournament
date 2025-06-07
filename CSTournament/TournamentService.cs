using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament;

public class TournamentService : ITournamentService
{
    private readonly Dictionary<Guid, Tournament> _tournaments = new();
    private readonly IPlayerService _playerService;

    public TournamentService(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public Results<Ok<Tournament>, BadRequest<string>, NotFound<string>> CreateTournament(TournamentCreateRequest req)
    {
        var newTournament = new Tournament(Guid.NewGuid(), req.Name, [], []);

        if (req.ParentId != null)
        {
            var parent = FindTournament(req.ParentId.Value);
            
            if (parent == null)
                return TypedResults.NotFound("Parent tournament not found.");
            
            if (GetDepth(parent) >= 5)
                return TypedResults.BadRequest("Max depth exceeded.");
            
            parent.SubTournaments.Add(newTournament);
        }
        else
        {
            _tournaments[newTournament.Id] = newTournament;
        }

        return TypedResults.Ok(newTournament);
    }

    public Ok<List<Tournament>> GetTournaments()
    {
        return TypedResults.Ok(_tournaments.Values.ToList());
    }

    public Results<Ok<Tournament>, NotFound> GetTournamentDetails(Guid id)
    {
        var tournament = FindTournament(id);
        
        return tournament != null ? TypedResults.Ok(tournament) : TypedResults.NotFound();
    }

    public Results<Ok<string>, NotFound<string>> DeleteTournament(Guid id)
    {
        var isDeleted = _tournaments.Remove(id);
        
        if (!isDeleted)
            isDeleted = DeleteFromSubTournaments(_tournaments.Values, id);
        
        return isDeleted ? TypedResults.Ok("") : TypedResults.NotFound("");
    }

    public Results<Ok, BadRequest<string>, NotFound<string>> RegisterPlayer(Guid tournamentId, Guid playerId)
    {
        if (!_playerService.PlayerExists(playerId))
            return TypedResults.NotFound("Player not found.");

        var tournament = FindTournament(tournamentId);
        if (tournament == null)
            return TypedResults.NotFound("Tournament not found.");

        var parent = FindParentTournament(_tournaments.Values, tournamentId);
        if (parent != null && !parent.PlayerIds.Contains(playerId))
        {
            return TypedResults.BadRequest("Player must be registered in parent tournament.");
        }

        if (!tournament.PlayerIds.Contains(playerId))
            tournament.PlayerIds.Add(playerId);

        return TypedResults.Ok();
    }

    // Helpers
    private Tournament? FindTournament(Guid id) => FindTournamentRecursive(_tournaments.Values, id);

    private static Tournament? FindTournamentRecursive(IEnumerable<Tournament> tournaments, Guid id)
    {
        foreach (var t in tournaments)
        {
            if (t.Id == id)
                return t;
            
            var found = FindTournamentRecursive(t.SubTournaments, id);
            
            if (found != null)
                return found;
        }
        
        return null;
    }

    private static bool DeleteFromSubTournaments(IEnumerable<Tournament> tournaments, Guid id)
    {
        foreach (var t in tournaments)
        {
            var match = t.SubTournaments.FirstOrDefault(x => x.Id == id);
            if (match != null)
            {
                t.SubTournaments.Remove(match);
                return true;
            }
            
            if (DeleteFromSubTournaments(t.SubTournaments, id))
                return true;
        }
        
        return false;
    }

    private static Tournament? FindParentTournament(IEnumerable<Tournament> tournaments, Guid childId)
    {
        foreach (var t in tournaments)
        {
            if (t.SubTournaments.Any(st => st.Id == childId))
                return t;
            
            var result = FindParentTournament(t.SubTournaments, childId);
            
            if (result != null)
                return result;
        }
        
        return null;
    }

    private static int GetDepth(Tournament tournament)
    {
        return 1 + (tournament.SubTournaments.Count == 0 ? 0 : tournament.SubTournaments.Max(GetDepth));
    }
}