using UnityEngine;
using TMPro;

public class BracketUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;

    void Start()
    {
        RefreshBracket();
    }

    // ✅ This is the method ScreenManager expects
    public void RefreshBracket()
    {
        // Update the bracket title based on current week from SeasonManager
        int currentWeek = SeasonManager.Instance != null ? SeasonManager.Instance.CurrentWeek : 1;

        if (titleText != null)
            titleText.text = $"CURRENT BRACKET - WEEK {currentWeek} RESULTS";

        Debug.Log($"Bracket refreshed for week {currentWeek}");
    }
}
