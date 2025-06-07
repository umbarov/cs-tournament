namespace CSTournament.Tests;

using Xunit;
using Moq;
using Microsoft.AspNetCore.Http.HttpResults;
using CSTournament.Interfaces;

public class TournamentServiceTests
{
    private readonly TournamentService _service;

    public TournamentServiceTests()
    {
        var mockPlayerService = new Mock<IPlayerService>();
        mockPlayerService.Setup(x => x.PlayerExists(It.IsAny<string>())).Returns(true);
        _service = new TournamentService(mockPlayerService.Object);
    }

    [Fact]
    public void CreateTournament_ShouldCreateRootTournament()
    {
        var result = _service.CreateTournament(new TournamentCreateRequest("SE-T", "Test Tournament", null));
        var okResult = Assert.IsType<Results<Ok<Tournament>, BadRequest<string>, NotFound<string>>>(result);
        Assert.Equal("Test Tournament", (okResult.Result as Ok<Tournament>)!.Value!.Name);
    }

    [Fact]
    public void RegisterPlayer_ShouldSucceed_WhenParentHasPlayer()
    {
        var username = "John";
        var parent = _service.CreateTournament(new TournamentCreateRequest("Parent", "Parent", null));

        Assert.IsType<Results<Ok<Tournament>, BadRequest<string>, NotFound<string>>>(parent);
        (parent.Result as Ok<Tournament>)?.Value!.PlayerUsernames.Add(username);
        
        var child = new Tournament("Stockholm-T", "Child", [], []);
        (parent.Result as Ok<Tournament>)?.Value!.SubTournaments.Add(child);

        var result = _service.RegisterPlayer(child.Id, username);
        Assert.IsType<Results<Ok, BadRequest<string>, NotFound<string>>>(result);
    }

    [Fact]
    public void RegisterPlayer_ShouldFail_WhenPlayerNotInParent()
    {
        var username = "john";
        var parent = _service.CreateTournament(new TournamentCreateRequest("SE-T","Parent", null));
        Assert.IsType<Results<Ok<Tournament>, BadRequest<string>, NotFound<string>>>(parent);
        
        var child = new Tournament("Stockholm", "Child", [], []);
        (parent.Result as Ok<Tournament>)?.Value!.SubTournaments.Add(child);

        var result = _service.RegisterPlayer(child.Id, username);
        var badResult = Assert.IsType<Results<Ok, BadRequest<string>, NotFound<string>>>(result);
        Assert.Equal("Player must be registered in parent tournament.", (badResult.Result as BadRequest<string>)!.Value);
    }
}
