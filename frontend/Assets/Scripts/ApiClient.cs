using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class TeamStatsSaveData
{
    public int wins;
    public int losses;
    public int ties;
    public int points;
    public int total_matches;
}

[Serializable]
public class TeamProgressionSaveData
{
    public int total_xp;
    public int current_level;
    public string tier;
    public List<Dictionary<string, object>> xp_history;
}

[Serializable]
public class TeamSaveData
{
    public string team_id;
    public string team_name;
    public int rating;
    public bool is_player_team;
    public int rank;
    public TeamStatsSaveData stats;
    public TeamProgressionSaveData progression;
}

[Serializable]
public class SeasonSaveData
{
    public string season_id;
    public int currentWeek;
    public int totalWeeks;
    public List<TeamSaveData> teams;
}

public class ApiClient : MonoBehaviour
{
    private readonly string baseUrl = "http://127.0.0.1:8000"; // change if hosted elsewhere

    public IEnumerator PostCreateSeason(Action<SeasonSaveData> callback)
    {
        string url = $"{baseUrl}/seasons";

        // Build request JSON
        var requestJson = new JSONObject();
        var teamsArray = new JSONArray();
        string[] teamNames = { "Jets", "Hawks", "Sharks", "Bears", "Lions", "Giants", "Eagles", "YOU" };

        foreach (string name in teamNames)
        {
            var teamJson = new JSONObject();
            teamJson["name"] = name;
            teamJson["rating"] = 0;
            teamsArray.Add(teamJson);
        }

        requestJson["teams"] = teamsArray;
        requestJson["player_team_name"] = "YOU";

        string payload = requestJson.ToString();
        Debug.Log("Payload: " + payload);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(payload);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"CreateSeason failed: {www.error}\n{www.downloadHandler.text}");
            }
            else
            {
                var json = JSONNode.Parse(www.downloadHandler.text);
                SeasonSaveData data = ParseSeasonSaveData(json);
                callback?.Invoke(data);
            }
        }
    }

    public IEnumerator PostSimulateWeek(SeasonSaveData currentData, Action<SeasonSaveData> callback)
    {
        if (string.IsNullOrEmpty(currentData.season_id))
        {
            Debug.LogError("SimulateWeek failed: season_id is null or empty.");
            yield break;
        }

        string url = $"{baseUrl}/seasons/{currentData.season_id}/simulate_week";
        Debug.Log($"SimulateWeek URL: {url}");

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(new byte[0]);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"SimulateWeek failed: {www.error}\n{www.downloadHandler.text}");
            }
            else
            {
                var json = JSONNode.Parse(www.downloadHandler.text);
                SeasonSaveData data = ParseSeasonSaveData(json);
                callback?.Invoke(data);
            }
        }
    }
    private SeasonSaveData ParseSeasonSaveData(JSONNode json)
    {
        SeasonSaveData data = new SeasonSaveData
        {
            season_id = json["season_id"],
            currentWeek = json["current_week"].AsInt,
            totalWeeks = json["total_weeks"].AsInt,
            teams = new List<TeamSaveData>()
        };

        foreach (JSONNode t in json["teams"].AsArray)
        {
            TeamSaveData team = new TeamSaveData
            {
                team_id = t["team_id"],
                team_name = t["team_name"],
                rating = t["rating"].AsInt,
                is_player_team = t["is_player_team"].AsBool,
                rank = t["rank"].AsInt,
                stats = new TeamStatsSaveData
                {
                    wins = t["stats"]["wins"].AsInt,
                    losses = t["stats"]["losses"].AsInt,
                    ties = t["stats"]["ties"].AsInt,
                    points = t["stats"]["points"].AsInt,
                    total_matches = t["stats"]["total_matches"].AsInt
                },
                progression = new TeamProgressionSaveData
                {
                    total_xp = t["progression"]["total_xp"].AsInt,
                    current_level = t["progression"]["current_level"].AsInt,
                    tier = t["progression"]["tier"],
                    xp_history = new List<Dictionary<string, object>>()
                }
            };

            // xp_history
            foreach (JSONNode entry in t["progression"]["xp_history"].AsArray)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (KeyValuePair<string, JSONNode> kv in entry.AsObject)
                {
                    dict[kv.Key] = kv.Value.Value; // Correct access
                }
                team.progression.xp_history.Add(dict);
            }

            data.teams.Add(team);
        }

        return data;
    }

}
