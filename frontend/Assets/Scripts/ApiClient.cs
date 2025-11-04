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
public class TierData
{
    public int min_xp;
    public int max_xp;
    public string display_name;
    public List<string> unlock_features;
}

// [Serializable]
// public class TeamProgressionSaveData
// {
//     public int total_xp;
//     public int current_level;
//     public string tier;
//     public List<Dictionary<string, object>> xp_history;
// }
[Serializable]
public class PlayerProgressionSaveData
{
    public string player_id;
    public int current_xp;
    public string current_tier;
    public Dictionary<string, TierData> tier_progression;
    public List<XPHistoryEntry> xp_history;
}

[Serializable]
public class XPHistoryEntry
{
    public string timestamp;
    public int xp_gained;
    public string source;
    public float facility_multiplier;
    public float coaching_bonus;
}


public class TeamSaveData
{
    public string team_id;
    public string player_id;
    public string team_name;
    public int rating;
    public bool is_player_team;
    public int wins;
    public int losses;
    public int rank

}


[Serializable]
public class SeasonSaveData
{
    public string season_id;
    public int week;
    public List<TeamSaveData> teams;
    public List<PlayerProgressionSaveData> player_progression;
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
            teamJson["team_name"] = name;
            teamJson["rating"] = 1000
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
            week = json["week"].AsInt,

            teams = new List<TeamSaveData>()
        };

        foreach (JSONNode t in json["teams"].AsArray)
        {
            TeamSaveData team = new TeamSaveData
            {
                team_id = t["team_id"],
                player_id = t["player_id"],
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
        if (json["player_progression"] != null)
        {
            data.player_progression = new List<PlayerProgressionSaveData>();
            foreach (JSONNode p in json["player_progression"].AsArray)
            {
                var prog = new PlayerProgressionSaveData
                {
                    player_id = p["player_id"],
                    current_xp = p["current_xp"].AsInt,
                    current_tier = p["current_tier"],
                    tier_progression = new Dictionary<string, TierData>(),
                    xp_history = new List<XPHistoryEntry>()
                };

                foreach (KeyValuePair<string, JSONNode> kv in p["tier_progression"].AsObject)
                {
                    var tierNode = kv.Value;
                    prog.tier_progression[kv.Key] = new TierData
                    {
                        min_xp = tierNode["min_xp"].AsInt,
                        max_xp = tierNode["max_xp"].AsInt,
                        display_name = tierNode["display_name"],
                        unlock_features = new List<string>()
                    };
                    foreach (JSONNode feat in tierNode["unlock_features"].AsArray)
                        prog.tier_progression[kv.Key].unlock_features.Add(feat);
                }

                foreach (JSONNode entry in p["xp_history"].AsArray)
                {
                    XPHistoryEntry history = new XPHistoryEntry
                    {
                        timestamp = entry["timestamp"],
                        xp_gained = entry["xp_gained"].AsInt,
                        source = entry["source"],
                        facility_multiplier = entry["facility_multiplier"].AsFloat,
                        coaching_bonus = entry["coaching_bonus"].AsFloat
                    };
                    prog.xp_history.Add(history);
                }

                data.player_progression.Add(prog);
            }
        }



        return data;
    }

}
