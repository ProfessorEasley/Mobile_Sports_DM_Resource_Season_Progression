using UnityEngine;
using TMPro;

public class XPUI : MonoBehaviour
{
    public TextMeshProUGUI currentXPText;
    public Transform xpListParent;
    public GameObject xpEntryPrefab;

    void OnEnable()
    {
        // subscribe safely if SeasonManager exists, otherwise poll in Start
        if (SeasonManager.Instance != null)
            SeasonManager.Instance.OnSeasonDataUpdated += RefreshXPHistory;
        else
            StartCoroutine(WaitAndSubscribe());
    }

    void OnDisable()
    {
        if (SeasonManager.Instance != null)
            SeasonManager.Instance.OnSeasonDataUpdated -= RefreshXPHistory;
    }

    private System.Collections.IEnumerator WaitAndSubscribe()
    {
        while (SeasonManager.Instance == null)
            yield return null;
        SeasonManager.Instance.OnSeasonDataUpdated += RefreshXPHistory;
        RefreshXPHistory();
    }

    public void RefreshXPHistory()
    {
        var sm = SeasonManager.Instance;
        if (sm == null) return;

        if (currentXPText != null)
            currentXPText.text = $"Current XP: {sm.PlayerXP}/1000";

        // rebuild xp history from ApiClient progression if available
        var prog = ApiClient.Instance?.PlayerProgressionSaveData;
        if (xpListParent == null || xpEntryPrefab == null || prog == null) return;

        // clear children
        for (int i = xpListParent.childCount - 1; i >= 0; i--)
            Destroy(xpListParent.GetChild(i).gameObject);

        foreach (var entry in prog.xp_history)
        {
            var go = Instantiate(xpEntryPrefab, xpListParent);
            var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = $"{entry.timestamp}: +{entry.xp_gained} XP ({entry.source})";
        }

        Debug.Log("XPUI: RefreshXPHistory called");
        Debug.Log("XP Count: " + prog?.xp_history?.Count);

    }
}
