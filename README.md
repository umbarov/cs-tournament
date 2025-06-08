# CS Tournament API

A RESTful web API for managing Counter-Strike tournaments and players, supporting hierarchical tournaments (up to 5 levels deep) and player registration with parent tournament constraints.

## 🚀 Features

- Register and manage players
- Create and organize tournaments with nested subtournaments (up to 5 levels)
- Register players only if they're already in parent tournaments
- RESTful API design
- In-memory storage
- Built with ASP.NET Core Minimal API
- Fully covered with unit tests
- Uses [Scalar](https://scalar.com) for API documentation

## 📦 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/) *(make sure to install .NET 9)*

## 🛠️ Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/umbarov/cs-tournament.git
cd cs-tournament
```

### 2. Build and Run the Application
```bash
dotnet run
```

### 3. Access Scalar UI
Navigate to:
```
https://localhost:5001/scalar
```
Use Scalar to explore and test the endpoints.

## 🧪 Running Unit Tests

From the root of the test project:
```bash
dotnet test
```
Tests are located in:
- `PlayerServiceTests.cs`
- `TournamentServiceTests.cs`

## 📘 API Overview

### Players
- `POST /api/players` — Add a player
- `GET /api/players` — List all players
- `GET /api/players/{username}` — Get player by Username
- `DELETE /api/players/{username}` — Delete player

### Tournaments
- `POST /api/tournaments` — Create a (sub)tournament
- `GET /api/tournaments` — List root tournaments
- `GET /api/tournaments/{id}` — Get tournament with all subtournaments
- `DELETE /api/tournaments/{id}` — Delete tournament
- `POST /api/tournaments/{id}/register?username=...` — Register player in a tournament

## 📂 Project Structure

- `Program.cs` — Main application file (Minimal API)
- `PlayerService` — Handles player logic
- `TournamentService` — Handles tournament logic
- `Interfaces` — Abstractions for DI and testability
- `CSTournament.Tests/` — xUnit test projects with Moq

## 🔒 Constraints & Rules

- Max 5 nested subtournament levels
- Players can only register in subtournaments if they're registered in the parent

---

