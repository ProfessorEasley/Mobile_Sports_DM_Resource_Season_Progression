// Assets/Scripts/SeasonManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance { get; private set; }

    public int CurrentWeek { get; private set; } = 1;
    public int TotalWeeks { get; private set; } = 10;
    public List<TeamData> Teams { get; private set; }
    public TeamData PlayerTeam { get; private set; }
    public List<string> XpHistory { get; private set; } = new List<string>();

    public int PlayerXP => PlayerTeam?.XP ?? 0;
    public int PlayerRank => Teams.OrderByDescending(t => t.Wins).ThenByDescending(t => t.Points).ToList().FindIndex(t => t.IsPlayerTeam) + 1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes if needed
            InitializeSeason();
        }
    }

    void InitializeSeason()
    {
        CurrentWeek = 1;
        Teams = new List<TeamData>
        {
            // Preloaded dummy data
            new TeamData { Name = "Jets", Wins = 0, Losses = 0, Points = 0, XP = 0 },
            new TeamData { Name = "Hawks", Wins = 0, Losses = 0, Points = 0, XP = 0 },
            new TeamData { Name = "Sharks", Wins = 0, Losses = 0, Points = 0, XP = 0 },
            new TeamData { Name = "Bears", Wins = 0, Losses = 0, Points = 0, XP = 0 },
            new TeamData { Name = "Lions", Wins = 0, Losses = 0, Points = 0, XP = 0 },
            new TeamData { Name = "Giants", Wins = 0, Losses = 0, Points = 0, XP = 0 },
            new TeamData { Name = "Eagles", Wins = 0, Losses = 0, Points = 0, XP = 0 },
            new TeamData { Name = "YOU", Wins = 0, Losses = 0, Points = 0, XP = 0, IsPlayerTeam = true }
        };
        PlayerTeam = Teams.First(t => t.IsPlayerTeam);
    }

    public (bool didPlayerWin, int xpGained, string opponentName) SimulateNextWeek()
    {
        if (CurrentWeek > TotalWeeks) return (false, 0, "None");

        // Simple simulation logic
        TeamData opponent = Teams.Where(t => !t.IsPlayerTeam).OrderBy(t => Random.value).First();
        bool playerWins = Random.value > 0.45; // 55% chance to win
        int xpGained = 0;

        if (playerWins)
        {
            PlayerTeam.Wins++;
            opponent.Losses++;
            PlayerTeam.Points += Random.Range(20, 40);
            xpGained = Random.Range(100, 150);
            XpHistory.Add($"Week {CurrentWeek} Win: +{xpGained} XP");
        }
        else
        {
            PlayerTeam.Losses++;
            opponent.Wins++;
            PlayerTeam.Points += Random.Range(7, 18);
            xpGained = Random.Range(40, 60);
            XpHistory.Add($"Week {CurrentWeek} Loss: +{xpGained} XP");
        }
        PlayerTeam.XP += xpGained;

        // Simulate other AI matches
        foreach (var team in Teams.Where(t => !t.IsPlayerTeam && t != opponent))
        {
            if (Random.value > 0.5) team.Wins++; else team.Losses++;
            team.Points += Random.Range(10, 35);
            team.XP += Random.Range(50, 120);
        }

        CurrentWeek++;
        return (playerWins, xpGained, opponent.Name);
    }
}