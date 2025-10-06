using System;

[Serializable]
public class TeamData
{
    public string teamName;
    public int wins;
    public int losses;
    public int experiencePoints;

    public TeamData(string name)
    {
        teamName = name;
        wins = 0;
        losses = 0;
        experiencePoints = 0;
    }

    public void RecordWin()
    {
        wins++;
        experiencePoints += 10; // Example XP for a win
    }

    public void RecordLoss()
    {
        losses++;
        experiencePoints += 5; // Example XP for a loss
    }

    public void ResetSeason()
    {
        wins = 0;
        losses = 0;
        experiencePoints = 0;
    }
}