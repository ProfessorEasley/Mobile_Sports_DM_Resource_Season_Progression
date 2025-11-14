using UnityEngine;
using TMPro;
using System.Linq;

public class BracketUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI titleText;
    public Transform tableParent;      
    public GameObject rowPrefab;       

    public void RefreshBracket()
    {
        var sm = SeasonManager.Instance;
        if (sm == null || sm.Teams.Count == 0)
        {
            Debug.LogWarning("BracketUI: No season data available yet.");
            return;
        }

        
        if (titleText != null)
            titleText.text = $"WEEK {sm.CurrentWeek} — TEAM STANDINGS";

        foreach (Transform child in tableParent)
            Destroy(child.gameObject);

        var standings = sm.Teams
            .OrderByDescending(t => t.stats.points)
            .ThenByDescending(t => t.stats.wins)
            .ToList();

        int rank = 1;

        foreach (var team in standings)
        {
            var row = Instantiate(rowPrefab, tableParent);

            var fields = row.GetComponentsInChildren<TextMeshProUGUI>();

           
            fields[0].text = rank.ToString();
            fields[1].text = team.team_name;
            fields[2].text = $"{team.stats.wins}-{team.stats.losses}";
            fields[3].text = team.stats.points.ToString();

            if (team.is_player_team)
                fields[1].color = Color.yellow;

            rank++;
        }

        Debug.Log("BracketUI: Standings updated.");
    }
}
