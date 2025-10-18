using UnityEngine;
using TMPro;

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

    [Header("Optional References (if available)")]
    public BracketUI bracketUI;
    public XPUI xpUI;

    private SeasonManager seasonManager;

    void Awake()
    {
        // Hook into the SeasonManager singleton
        seasonManager = SeasonManager.Instance;

        if (seasonManager == null)
        {
            Debug.LogError("ScreenManager: SeasonManager not found in scene!");
            return;
        }

        // Whenever season data changes, update all screens
        seasonManager.OnSeasonDataUpdated += UpdateAllScreens;
    }

    void Start()
    {
        ShowHub();
        UpdateAllScreens();
    }

    public void ShowHub()
    {
        ShowScreen(screenHub);
        UpdateHubDisplay();
    }

    public void ShowBracket()
    {
        ShowScreen(screenBracket);
        if (bracketUI != null)
            bracketUI.RefreshBracket();
    }

    public void ShowXP()
    {
        ShowScreen(screenXP);
        if (xpUI != null)
            xpUI.RefreshXPHistory();
    }

    public void ShowSimFeedback()
    {
        ShowScreen(screenSimFeedback);
    }

    private void ShowScreen(GameObject screen)
    {
        screenHub.SetActive(false);
        screenBracket.SetActive(false);
        screenXP.SetActive(false);
        screenSimFeedback.SetActive(false);

        screen.SetActive(true);
    }

    public void UpdateAllScreens()
    {
        if (seasonManager == null)
            seasonManager = SeasonManager.Instance;

        UpdateHubDisplay();

        if (xpUI != null)
            xpUI.RefreshXPHistory();

        if (bracketUI != null)
            bracketUI.RefreshBracket();
    }

    public void UpdateHubDisplay()
    {
        if (seasonManager == null)
        {
            Debug.LogWarning("ScreenManager: SeasonManager not initialized yet.");
            return;
        }

        if (xpText != null)
            xpText.text = $"XP: {seasonManager.PlayerXP}/1000";

        if (weekText != null)
            weekText.text = $"Week: {seasonManager.CurrentWeek}/10";
    }
}
