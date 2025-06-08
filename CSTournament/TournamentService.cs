using CSTournament.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament;

public class TournamentService : ITournamentService
{
    private readonly Dictionary<string, Tournament> _tournaments = new(StringComparer.OrdinalIgnoreCase);
    private readonly IPlayerService _playerService;
    private const int MaxNestingDepth = 5;

    public TournamentService(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public Results<Ok<Tournament>, BadRequest<string>, NotFound<string>> CreateTournament(TournamentCreateRequest req)
    {
        var newTournament = new Tournament(req.Id, req.Name, req.ParentId, [], []);
        
        if (req.ParentId != null)
        {
            var parent = FindTournament(req.ParentId);

            if (parent == null)
                return TypedResults.NotFound($"Parent {req.ParentId} tournament not found.");
            
            if (GetDepth(parent) >= MaxNestingDepth)
                return TypedResults.BadRequest("Max depth exceeded.");
            
            parent.SubTournaments.Add(newTournament);
        }
        else
        {
            _tournaments[newTournament.Id] = newTournament;
        }

        return TypedResults.Ok(newTournament);
    }

    public Ok<IEnumerable<Tournament>> GetTournaments()
    {
        return TypedResults.Ok(_tournaments.Values as IEnumerable<Tournament>);
    }

    public Results<Ok<Tournament>, NotFound> GetTournamentDetails(string id)
    {
        var tournament = FindTournament(id);
        
        return tournament != null ? TypedResults.Ok(tournament) : TypedResults.NotFound();
    }

    public Results<Ok<string>, NotFound<string>> DeleteTournament(string id)
    {
        var isDeleted = _tournaments.Remove(id);
        
        if (!isDeleted)
            isDeleted = DeleteFromSubTournaments(_tournaments.Values, id);
        
        return isDeleted
            ? TypedResults.Ok($"Tournament {id} has been deleted.")
            : TypedResults.NotFound($"Tournament {id} not found.");
    }

    public Results<Ok, BadRequest<string>, NotFound<string>> RegisterPlayer(string tournamentId, string username)
    {
        if (!_playerService.PlayerExists(username))
            return TypedResults.NotFound($"Player {username} not found.");

        var tournament = FindTournament(tournamentId);
        if (tournament == null)
            return TypedResults.NotFound($"Tournament {tournamentId} not found.");

        var parent = FindParentTournament(_tournaments.Values, tournamentId);
        if (parent != null && !parent.PlayerUsernames.Contains(username, StringComparer.OrdinalIgnoreCase))
        {
            return TypedResults.BadRequest("Player must be registered in parent tournament.");
        }

        if (!tournament.PlayerUsernames.Contains(username, StringComparer.OrdinalIgnoreCase))
            tournament.PlayerUsernames.Add(username);

        return TypedResults.Ok();
    }

    // Helpers
    private Tournament? FindTournament(string id) => FindTournamentRecursive(_tournaments.Values, id);

    private static Tournament? FindTournamentRecursive(IEnumerable<Tournament> tournaments, string id)
    {
        foreach (var t in tournaments)
        {
            if (t.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                return t;
            
            var found = FindTournamentRecursive(t.SubTournaments, id);
            
            if (found != null)
                return found;
        }
        
        return null;
    }

    private static bool DeleteFromSubTournaments(IEnumerable<Tournament> tournaments, string id)
    {
        foreach (var t in tournaments)
        {
            var match = t.SubTournaments
                .FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
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

    private static Tournament? FindParentTournament(IEnumerable<Tournament> tournaments, string childId)
    {
        foreach (var t in tournaments)
        {
            if (t.SubTournaments.Any(st => st.Id.Equals(childId, StringComparison.OrdinalIgnoreCase)))
                return t;
            
            var result = FindParentTournament(t.SubTournaments, childId);
            
            if (result != null)
                return result;
        }
        
        return null;
    }

    private int GetDepth(Tournament tournament)
    {
        if (tournament.ParentId == null)
            return 1;
        
        var parent = FindTournament(tournament.ParentId)!;
        
        return 1 + GetDepth(parent);
    }
}