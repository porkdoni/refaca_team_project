using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FarmManager : MonoBehaviour
{
    public List<ZoneBoundary> zoneBoundaries = new List<ZoneBoundary>();
    public List<int> unlockedZoneIds = new List<int>();
    public int coins = 0;

    public TextMeshProUGUI coinText;

    void Start()
    {
        UnlockZone(1);
        UpdateCoinUI();
    }

    public bool IsPositionInUnlockedZone(Vector3 position)
    {
        foreach (int zoneId in unlockedZoneIds)
        {
            ZoneBoundary zoneBoundary = zoneBoundaries.Find(z => z.zoneId == zoneId);
            if (zoneBoundary != null && zoneBoundary.bounds.Contains(new Vector2(position.x, position.z)))
            {
                return true;
            }
        }
        return false;
    }

    public void UnlockZone(int zoneId)
    {
        ZoneBoundary zoneBoundary = zoneBoundaries.Find(z => z.zoneId == zoneId);
        if (zoneBoundary != null && coins >= zoneBoundary.unlockCost && !unlockedZoneIds.Contains(zoneId))
        {
            coins -= zoneBoundary.unlockCost;
            unlockedZoneIds.Add(zoneId);
            Debug.Log("Zone " + zoneId + " unlocked!");
            UpdateCoinUI();
        }
        else
        {
            Debug.Log("Not enough coins or already unlocked.");
        }
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = FormatCoins(coins);
        }
    }

    private string FormatCoins(int amount)
    {
        if (amount >= 1000000)
        {
            float thousands = amount / 1000f;
            return Mathf.RoundToInt(thousands) + "K";
        }
        else
        {
            return amount.ToString();
        }
    }
}