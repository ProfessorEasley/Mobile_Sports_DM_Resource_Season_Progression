using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance { get; private set; }

    private SeasonSaveData seasonData;
    private ApiClient apiClient;
    private ProgressionUIController uiController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        apiClient = gameObject.AddComponent<ApiClient>();
        uiController = FindObjectOfType<ProgressionUIController>();

        StartCoroutine(apiClient.PostCreateSeason(data =>
        {
            seasonData = data;
            Debug.Log("Season created from API.");

            // Notify UI that data is ready
            uiController?.OnSeasonDataReady();
        }));
    }

    public int CurrentWeek => seasonData?.currentWeek ?? 0;
    public int TotalWeeks => seasonData?.totalWeeks ?? 0;

    public List<TeamSaveData> Teams => seasonData?.teams ?? new List<TeamSaveData>();
    public TeamSaveData PlayerTeam => seasonData?.teams?.FirstOrDefault(t => t.is_player_team);

    public void SimulateNextWeek(Action<SeasonSaveData> callback)
    {
        if (seasonData == null)
        {
            Debug.LogError("Cannot simulate week: Season data not initialized");
            return;
        }

        StartCoroutine(apiClient.PostSimulateWeek(seasonData, data =>
        {
            seasonData = data;
            callback?.Invoke(seasonData);
        }));
    }

    public int PlayerXP => PlayerTeam?.progression?.total_xp ?? 0;

    public int PlayerRank
    {
        get
        {
            if (Teams == null || Teams.Count == 0) return 0;

            // Sort by points first, then wins (as backend uses points and wins for ranking)
            var sorted = Teams.OrderByDescending(t => t.stats.points)
                              .ThenByDescending(t => t.stats.wins)
                              .ToList();

            var playerIndex = sorted.FindIndex(t => t.is_player_team);
            return playerIndex >= 0 ? playerIndex + 1 : 0;
        }
    }

    public List<string> XpHistory
    {
        get
        {
            var player = PlayerTeam;
            if (player?.progression?.xp_history == null) return new List<string>();

            List<string> history = new List<string>();
            foreach (var entry in player.progression.xp_history)
            {
                string week = entry.ContainsKey("week") ? entry["week"].ToString() : "?";
                string result = entry.ContainsKey("event") ? entry["event"].ToString() : "Unknown";
                string xp = entry.ContainsKey("xp_change") ? entry["xp_change"].ToString() : "0";

                history.Add($"Week {week} {result.ToUpper()}: +{xp} XP");
            }
            return history;
        }
    }
}
