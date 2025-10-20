using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

// Namespace wrapper
using FMGSeasonProgression;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Store all active seasons in memory
var activeSeasons = new Dictionary<string, SeasonController>();

// ---- POST /seasons ----
app.MapPost("/seasons", (FMGSeasonProgression.CreateSeasonRequest request) =>
{
    try
    {
        if (request.Teams == null || request.Teams.Count == 0)
            return Results.BadRequest("No teams provided.");

        string id = $"S_{Guid.NewGuid().ToString()[..8]}";
        var teams = new List<FMGSeasonProgression.TeamData>();

        foreach (var d in request.Teams)
        {
            try
            {
                string name = d.ContainsKey("name") ? d["name"].ToString() ?? "Unnamed" : "Unnamed";
                int rating = 1000;
                if (d.ContainsKey("rating") && int.TryParse(d["rating"].ToString(), out int parsed))
                    rating = parsed;

                teams.Add(new FMGSeasonProgression.TeamData
                {
                    TeamId = $"T_{Guid.NewGuid().ToString()[..6]}",
                    TeamName = name,
                    Rating = rating,
                    IsPlayerTeam = name == request.PlayerTeamName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing team: {ex.Message}");
            }
        }

        var state = new FMGSeasonProgression.SeasonState { SeasonId = id, Teams = teams };
        var ctrl = new FMGSeasonProgression.SeasonController(state);
        activeSeasons[id] = ctrl;

        FMGSeasonProgression.LogHelper.LogEvent("SeasonStart", new { season_id = id, team_count = teams.Count });
        return Results.Json(ctrl.State);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating season: {ex}");
        return Results.Problem($"Server error: {ex.Message}");
    }
});

// ---- POST /seasons/{id}/simulate_week ----
app.MapPost("/seasons/{id}/simulate_week", (string id) =>
{
    if (!activeSeasons.TryGetValue(id, out var ctrl))
        return Results.NotFound($"Season {id} not found.");

    ctrl.SimulateWeek();
    return Results.Json(ctrl.State);
});

// ---- GET /seasons/{id} ----
app.MapGet("/seasons/{id}", (string id) =>
{
    if (!activeSeasons.TryGetValue(id, out var ctrl))
        return Results.NotFound($"Season {id} not found.");
    return Results.Json(ctrl.State);
});

app.Run();

namespace FMGSeasonProgression
{
    public class TeamData
    {
        public string TeamId { get; set; } = "";
        public string TeamName { get; set; } = "";
        public int Rating { get; set; }
        public bool IsPlayerTeam { get; set; }
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
    }

    public class SeasonState
    {
        public string SeasonId { get; set; } = "";
        public List<TeamData> Teams { get; set; } = new();
        public int Week { get; set; } = 0;
    }

    public static class LogHelper
    {
        public static void LogEvent(string eventName, object details)
        {
            var json = JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = false });
            Console.WriteLine($"[{DateTime.Now}] {eventName}: {json}");
        }
    }

    public class SeasonController
    {
        public SeasonState State { get; private set; }

        public SeasonController(SeasonState state)
        {
            State = state;
        }

        public void SimulateWeek()
        {
            var rnd = new Random();
            var teams = State.Teams.OrderBy(t => rnd.Next()).ToList();

            for (int i = 0; i < teams.Count; i += 2)
            {
                if (i + 1 >= teams.Count) break;
                var teamA = teams[i];
                var teamB = teams[i + 1];

                var aWinProb = 1.0 / (1.0 + Math.Pow(10, (teamB.Rating - teamA.Rating) / 400.0));
                bool aWins = rnd.NextDouble() < aWinProb;

                if (aWins)
                {
                    teamA.Wins++;
                    teamB.Losses++;
                    teamA.Rating += 10;
                    teamB.Rating -= 10;
                }
                else
                {
                    teamB.Wins++;
                    teamA.Losses++;
                    teamB.Rating += 10;
                    teamA.Rating -= 10;
                }
            }

            State.Week++;
            LogHelper.LogEvent("SimulateWeek", new { week = State.Week, teams = State.Teams.Count });
        }
    }

    public class CreateSeasonRequest
    {
        [JsonPropertyName("teams")]
        public List<Dictionary<string, object>> Teams { get; set; } = new();

        [JsonPropertyName("playerTeamName")]
        public string PlayerTeamName { get; set; } = "";
    }
}

// Top-level program (after namespace)
