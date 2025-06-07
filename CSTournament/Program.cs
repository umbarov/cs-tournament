using CSTournament;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IPlayerService, PlayerService>();
builder.Services.AddSingleton<ITournamentService, TournamentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Player endpoints
app.MapPost("/players", (IPlayerService service, Player player) => service.AddPlayer(player));
app.MapGet("/players", (IPlayerService service) => service.GetPlayers());
app.MapGet("/players/{id:guid}", (IPlayerService service, Guid id) => service.GetPlayer(id));
app.MapDelete("/players/{id:guid}", (IPlayerService service, Guid id) => service.DeletePlayer(id));

// Tournament endpoints
app.MapPost("/tournaments", (ITournamentService service, TournamentCreateRequest req) => service.CreateTournament(req));
app.MapGet("/tournaments", (ITournamentService service) => service.GetTournaments());
app.MapGet("/tournaments/{id:guid}", (ITournamentService service, Guid id) => service.GetTournamentDetails(id));
app.MapDelete("/tournaments/{id:guid}", (ITournamentService service, Guid id) => service.DeleteTournament(id));
app.MapPost("/tournaments/{id:guid}/register", (ITournamentService service, Guid id, Guid playerId) => service.RegisterPlayer(id, playerId));

app.Run();
