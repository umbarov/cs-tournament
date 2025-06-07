namespace CSTournament.Tests;

using Xunit;
using Microsoft.AspNetCore.Http.HttpResults;

public class PlayerServiceTests
{
    private readonly PlayerService _service = new();

    [Fact]
    public void AddPlayer_ShouldAddNewPlayer()
    {
        var player = new Player("john", "John", 25);
        var result = _service.AddPlayer(player);

        var okResult = Assert.IsType<Results<Ok<Player>, BadRequest<string>>>(result);
        var addedPlayer = (okResult.Result as Ok<Player>)?.Value!;
        
        Assert.NotNull(addedPlayer);
        Assert.Equal("john", addedPlayer.Username);
        Assert.Equal("John", addedPlayer.Name);
        Assert.Equal(25, addedPlayer.Age);
    }

    [Fact]
    public void AddPlayer_ShouldRejectDuplicateId()
    {
        var player1 = new Player("alice", "Alice", 30);
        var player2 = new Player("bob", "Bob", 28);

        _service.AddPlayer(player1);
        var result = _service.AddPlayer(player2);

        Assert.IsType<Results<Ok<Player>, BadRequest<string>>>(result);
    }

    [Fact]
    public void GetPlayer_ShouldReturnPlayer_WhenExists()
    {
        var username = "eve";
        var player = new Player(username, "Eve", 21);
        _service.AddPlayer(player);

        var result = _service.GetPlayer(username);
        var okResult = Assert.IsType<Results<Ok<Player>, NotFound>>(result);
        Assert.Equal("Eve", (okResult.Result as Ok<Player>)!.Value!.Name);
    }

    [Fact]
    public void GetPlayer_ShouldReturnNotFound_WhenMissing()
    {
        var result = _service.GetPlayer("notFound");
        Assert.IsType<Results<Ok<Player>, NotFound>>(result);
        Assert.IsType<NotFound>(result.Result as NotFound);
    }

    [Fact]
    public void DeletePlayer_ShouldRemovePlayer()
    {
        var username = "mike";
        _service.AddPlayer(new Player(username, "Mike", 26));

        var result = _service.DeletePlayer(username);
        Assert.IsType<Results<Ok, NotFound>>(result);
    }

    [Fact]
    public void DeletePlayer_ShouldReturnNotFound_WhenMissing()
    {
        var result = _service.DeletePlayer("notFound");
        Assert.IsType<Results<Ok, NotFound>>(result);
        Assert.IsType<NotFound>(result.Result as NotFound);
    }

    [Fact]
    public void PlayerExists_ShouldReturnTrue_WhenExists()
    {
        var username = "lara";
        _service.AddPlayer(new Player("lara", "Lara", 32));
        Assert.True(_service.PlayerExists(username));
    }

    [Fact]
    public void PlayerExists_ShouldReturnFalse_WhenNotExists()
    {
        Assert.False(_service.PlayerExists("notFound"));
    }
}
