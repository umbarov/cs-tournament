using Microsoft.AspNetCore.Http.HttpResults;

namespace CSTournament.Tests;

using System;
using Xunit;

public class PlayerServiceTests
{
    private readonly PlayerService _service = new();

    [Fact]
    public void AddPlayer_ShouldAddNewPlayer()
    {
        var player = new Player(Guid.NewGuid(), "John", 25);
        var result = _service.AddPlayer(player);

        var okResult = Assert.IsType<Results<Ok<Player>, BadRequest<string>>>(result);
        Assert.Equal("John", (okResult.Result as Ok<Player>)?.Value?.Name);
    }

    [Fact]
    public void AddPlayer_ShouldRejectDuplicateId()
    {
        var id = Guid.NewGuid();
        var player1 = new Player(id, "Alice", 30);
        var player2 = new Player(id, "Bob", 28);

        _service.AddPlayer(player1);
        var result = _service.AddPlayer(player2);

        Assert.IsType<Results<Ok<Player>, BadRequest<string>>>(result);
    }

    [Fact]
    public void GetPlayer_ShouldReturnPlayer_WhenExists()
    {
        var id = Guid.NewGuid();
        var player = new Player(id, "Eve", 21);
        _service.AddPlayer(player);

        var result = _service.GetPlayer(id);
        var okResult = Assert.IsType<Results<Ok<Player>, NotFound>>(result);
        Assert.Equal("Eve", (okResult.Result as Ok<Player>)!.Value!.Name);
    }

    [Fact]
    public void GetPlayer_ShouldReturnNotFound_WhenMissing()
    {
        var result = _service.GetPlayer(Guid.NewGuid());
        Assert.IsType<Results<Ok<Player>, NotFound>>(result);
    }

    [Fact]
    public void DeletePlayer_ShouldRemovePlayer()
    {
        var id = Guid.NewGuid();
        _service.AddPlayer(new Player(id, "Mike", 26));

        var result = _service.DeletePlayer(id);
        Assert.IsType<Results<Ok, NotFound>>(result);
    }

    [Fact]
    public void DeletePlayer_ShouldReturnNotFound_WhenMissing()
    {
        var result = _service.DeletePlayer(Guid.NewGuid());
        Assert.IsType<Results<Ok, NotFound>>(result);
    }

    [Fact]
    public void PlayerExists_ShouldReturnTrue_WhenExists()
    {
        var id = Guid.NewGuid();
        _service.AddPlayer(new Player(id, "Lara", 32));
        Assert.True(_service.PlayerExists(id));
    }

    [Fact]
    public void PlayerExists_ShouldReturnFalse_WhenNotExists()
    {
        Assert.False(_service.PlayerExists(Guid.NewGuid()));
    }
}
