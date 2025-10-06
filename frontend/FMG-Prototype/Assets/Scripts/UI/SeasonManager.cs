using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    public int currentWeek;
    public List<TeamData> teamStandings;
    public int totalXP;

    void Start()
    {
        currentWeek = 1;
        teamStandings = new List<TeamData>();
        totalXP = 0;
    }

    public void AdvanceWeek()
    {
        currentWeek++;
        // Logic to update standings and XP based on the current week
    }

    public void UpdateStandings(TeamData teamData)
    {
        // Logic to update team standings
    }

    public void AddXP(int xp)
    {
        totalXP += xp;
        // Logic to handle XP rewards and milestones
    }
}