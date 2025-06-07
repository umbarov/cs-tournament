namespace CSTournament;

public record Tournament(string Id, string Name, List<string> PlayerUsernames, List<Tournament> SubTournaments);