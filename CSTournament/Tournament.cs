namespace CSTournament;

public record Tournament(string Id, string Name, string? ParentId, List<string> PlayerUsernames, List<Tournament> SubTournaments);