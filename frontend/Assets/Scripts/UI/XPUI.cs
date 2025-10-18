using UnityEngine;
using TMPro;

public class XPUI : MonoBehaviour
{
    public TextMeshProUGUI currentXPText;
    public Transform xpListParent;
    public GameObject xpEntryPrefab;

    void OnEnable() => SeasonManager.Instance.OnSeasonDataUpdated += RefreshXPHistory;
    void OnDisable() => SeasonManager.Instance.OnSeasonDataUpdated -= RefreshXPHistory;

    public void RefreshXPHistory()
    {
        var sm = SeasonManager.Instance;
        currentXPText.text = $"Current XP: {sm.PlayerXP}/1000";

        // TODO: Clear and rebuild XP history list
        // foreach (Transform child in xpListParent)
        //     Destroy(child.gameObject);
        //
        // foreach (var entry in sm.XPHistory)
        // {
        //     var item = Instantiate(xpEntryPrefab, xpListParent);
        //     item.GetComponentInChildren<TextMeshProUGUI>().text = entry;
        // }
    }
}
