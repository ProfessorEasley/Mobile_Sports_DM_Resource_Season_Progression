using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimulationFeedbackUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI opponentText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI xpEarnedText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI offenseText;
    public TextMeshProUGUI defenseText;

    [Header("Buttons")]
    public Button btnSimNextWeek;
    public Button btnBackToHub;

    private SeasonDataManager seasonDataManager;
    private ScreenManager screenManager;

    void Start()
    {
        seasonDataManager = SeasonDataManager.Instance;
        screenManager = FindObjectOfType<ScreenManager>();

        btnSimNextWeek.onClick.AddListener(OnSimulateNextWeek);
        btnBackToHub.onClick.AddListener(() => screenManager.ShowHub());
    }

    public void OnSimulateNextWeek()
    {
        if (seasonDataManager.CurrentWeek >= 10)
        {
            Debug.Log("Season complete!");
            return;
        }

        string[] opponents = { "Jets", "Hawks", "Lions", "Bulls", "Panthers", "Eagles" };
        string opponent = opponents[Random.Range(0, opponents.Length)];
        bool win = Random.value > 0.5f;
        int xpGained = Random.Range(50, 150);

        seasonDataManager.NextWeek();
        seasonDataManager.AddXP(xpGained);
        seasonDataManager.AddXPHistory($"Week {seasonDataManager.CurrentWeek} {(win ? "Win" : "Loss")}", xpGained);

        ShowSimulationResult(win, xpGained, opponent);
        screenManager.UpdateHubDisplay();

        StartCoroutine(ReturnToHubAfterDelay(1.5f));
    }

    public void ShowSimulationResult(bool didPlayerWin, int xpGained, string opponentName)
    {
        int currentWeek = seasonDataManager.CurrentWeek;
        titleText.text = $"MATCH SIMULATION RESULT - WEEK {currentWeek}";
        opponentText.text = $"Opponent: {opponentName}";
        resultText.text = didPlayerWin ? "Result: WIN" : "Result: LOSS";
        resultText.color = didPlayerWin ? Color.green : new Color(1f, 0.8f, 0.4f);
        xpEarnedText.text = $"XP Earned: +{xpGained}";

        int offenseBoost = Random.Range(5, 15);
        int defenseBoost = Random.Range(3, 10);
        offenseText.text = $"Offense: +{offenseBoost}%";
        defenseText.text = $"Defense: +{defenseBoost}%";

        if (seasonDataManager.PlayerXP >= 1000)
            rewardText.text = "Reward Progress: Bronze → Silver (Unlocked!)";
        else
            rewardText.text = $"Reward Progress: Bronze ({seasonDataManager.PlayerXP}/1000)";
    }

    private IEnumerator ReturnToHubAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        screenManager.ShowHub();
    }
}
