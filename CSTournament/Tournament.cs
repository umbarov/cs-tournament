namespace CSTournament;

public record Tournament(Guid Id, string Name, List<Guid> PlayerIds, List<Tournament> SubTournaments);