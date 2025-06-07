using CSTournament;
using CSTournament.Interfaces;
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
app.MapGet("/players/{username}", (IPlayerService service, string username) => service.GetPlayer(username));
app.MapDelete("/players/{username}", (IPlayerService service, string username) => service.DeletePlayer(username));

// Tournament endpoints
app.MapPost("/tournaments", (ITournamentService service, TournamentCreateRequest req) => service.CreateTournament(req));
app.MapGet("/tournaments", (ITournamentService service) => service.GetTournaments());
app.MapGet("/tournaments/{id}", (ITournamentService service, string id) => service.GetTournamentDetails(id));
app.MapDelete("/tournaments/{id}", (ITournamentService service, string id) => service.DeleteTournament(id));
app.MapPost("/tournaments/{id}/register", (ITournamentService service, string id, string username) => service.RegisterPlayer(id, username));

app.Run();
