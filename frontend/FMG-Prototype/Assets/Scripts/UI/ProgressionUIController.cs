using UnityEngine;

public class ProgressionUIController : MonoBehaviour
{
    // Reference to the UI elements for the progression system
    public GameObject facilitiesOverviewScreen;
    public GameObject simulationPreviewScreen;
    public GameObject bracketStandingsScreen;
    public GameObject weeklyResultsScreen;
    public GameObject xpRewardMilestoneTrackerScreen;

    private void Start()
    {
        // Initialize the UI flow
        ShowFacilitiesOverview();
    }

    public void ShowFacilitiesOverview()
    {
        HideAllScreens();
        facilitiesOverviewScreen.SetActive(true);
    }

    public void ShowSimulationPreview()
    {
        HideAllScreens();
        simulationPreviewScreen.SetActive(true);
    }

    public void ShowBracketStandings()
    {
        HideAllScreens();
        bracketStandingsScreen.SetActive(true);
    }

    public void ShowWeeklyResults()
    {
        HideAllScreens();
        weeklyResultsScreen.SetActive(true);
    }

    public void ShowXPRewardMilestoneTracker()
    {
        HideAllScreens();
        xpRewardMilestoneTrackerScreen.SetActive(true);
    }

    private void HideAllScreens()
    {
        facilitiesOverviewScreen.SetActive(false);
        simulationPreviewScreen.SetActive(false);
        bracketStandingsScreen.SetActive(false);
        weeklyResultsScreen.SetActive(false);
        xpRewardMilestoneTrackerScreen.SetActive(false);
    }
}