namespace CSTournament.Tests;

using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http.HttpResults;

public class TournamentServiceTests
{
    private readonly TournamentService _service;

    public TournamentServiceTests()
    {
        var mockPlayerService = new Mock<IPlayerService>();
        mockPlayerService.Setup(x => x.PlayerExists(It.IsAny<Guid>())).Returns(true);
        _service = new TournamentService(mockPlayerService.Object);
    }

    [Fact]
    public void CreateTournament_ShouldCreateRootTournament()
    {
        var result = _service.CreateTournament(new TournamentCreateRequest("Test Tournament", null));
        var okResult = Assert.IsType<Results<Ok<Tournament>, BadRequest<string>, NotFound<string>>>(result);
        Assert.Equal("Test Tournament", (okResult.Result as Ok<Tournament>)!.Value!.Name);
    }

    [Fact]
    public void RegisterPlayer_ShouldSucceed_WhenParentHasPlayer()
    {
        var playerId = Guid.NewGuid();
        var parent = _service.CreateTournament(new TournamentCreateRequest("Parent", null));

        Assert.IsType<Results<Ok<Tournament>, BadRequest<string>, NotFound<string>>>(parent);
        (parent.Result as Ok<Tournament>)?.Value!.PlayerIds.Add(playerId);
        
        var child = new Tournament(Guid.NewGuid(), "Child", [], []);
        (parent.Result as Ok<Tournament>)?.Value!.SubTournaments.Add(child);

        var result = _service.RegisterPlayer(child.Id, playerId);
        Assert.IsType<Results<Ok, BadRequest<string>, NotFound<string>>>(result);
    }

    [Fact]
    public void RegisterPlayer_ShouldFail_WhenPlayerNotInParent()
    {
        var playerId = Guid.NewGuid();
        var parent = _service.CreateTournament(new TournamentCreateRequest("Parent", null));
        Assert.IsType<Results<Ok<Tournament>, BadRequest<string>, NotFound<string>>>(parent);
        
        var child = new Tournament(Guid.NewGuid(), "Child", new List<Guid>(), new List<Tournament>());
        (parent.Result as Ok<Tournament>)?.Value!.SubTournaments.Add(child);

        var result = _service.RegisterPlayer(child.Id, playerId);
        var badResult = Assert.IsType<Results<Ok, BadRequest<string>, NotFound<string>>>(result);
        Assert.Equal("Player must be registered in parent tournament.", (badResult.Result as BadRequest<string>)!.Value);
    }
}
