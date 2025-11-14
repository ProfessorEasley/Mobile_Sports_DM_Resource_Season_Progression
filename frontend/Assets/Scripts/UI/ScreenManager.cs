using UnityEngine;
using TMPro;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject screenHub;
    public GameObject screenBracket;
    public GameObject screenXP;
    public GameObject screenSimFeedback;

    [Header("Hub UI References")]
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI weekText;
    public TextMeshProUGUI remainingMatchesText;
    public TextMeshProUGUI rankText; 
    public TextMeshProUGUI tierText; 


    [Header("Optional References (if available)")]
    public BracketUI bracketUI;
    public XPUI xpUI;

    private SeasonManager seasonManager;

 
    void Awake()
    {
        StartCoroutine(WaitForSeasonManager());
    }

    private IEnumerator WaitForSeasonManager()
    {
        while (SeasonManager.Instance == null)
            yield return null;

        seasonManager = SeasonManager.Instance;

        seasonManager.OnSeasonDataUpdated += UpdateAllScreens;

        Debug.Log("ScreenManager: Subscribed to SeasonManager events");

        UpdateAllScreens();
    }

    private void OnDestroy()
    {
        if (seasonManager != null)
            seasonManager.OnSeasonDataUpdated -= UpdateAllScreens;
    }

   
    void Start()
    {
        ShowHub();
    }

    public void ShowHub()
    {
        ShowScreen(screenHub);
        UpdateHubDisplay();
    }

    public void ShowBracket()
    {
        ShowScreen(screenBracket);
        bracketUI?.RefreshBracket();
    }

    public void ShowXP()
    {
        ShowScreen(screenXP);
        xpUI?.RefreshXPHistory();
    }

    public void ShowSimFeedback()
    {
        ShowScreen(screenSimFeedback);
    }

    private void ShowScreen(GameObject screen)
    {
        if (screenHub != null) screenHub.SetActive(false);
        if (screenBracket != null) screenBracket.SetActive(false);
        if (screenXP != null) screenXP.SetActive(false);
        if (screenSimFeedback != null) screenSimFeedback.SetActive(false);

        screen.SetActive(true);
    }


    public void UpdateAllScreens()
    {
        if (seasonManager == null) return;

        Debug.Log("ScreenManager: Updating ALL screens");

        UpdateHubDisplay();
        xpUI?.RefreshXPHistory();
        bracketUI?.RefreshBracket();
    }

    public void UpdateHubDisplay()
    {
        if (seasonManager == null)
        {
            Debug.LogWarning("ScreenManager: SeasonManager not initialized yet");
            return;
        }

        if (xpText != null)
            xpText.text = $"XP: {seasonManager.PlayerXP}/1000";

        if (weekText != null)
            weekText.text = $"Week: {seasonManager.CurrentWeek}/10";
        if (remainingMatchesText != null)
        {
            int remaining = 10 - seasonManager.CurrentWeek;
            remainingMatchesText.text = $"Remaining: {remaining}";
        }

        if (tierText != null)
            tierText.text = $"Tier: {seasonManager.PlayerTier}";

        Debug.Log($"ScreenManager: Hub Updated → XP={seasonManager.PlayerXP}, Week={seasonManager.CurrentWeek}");

        if (rankText != null)
        rankText.text = $"Rank: {seasonManager.PlayerRank} / {seasonManager.Teams.Count}";

        Debug.Log($"ScreenManager: Hub Updated → XP={seasonManager.PlayerXP}, Week={seasonManager.CurrentWeek}, Rank={seasonManager.PlayerRank}");
    }
}
