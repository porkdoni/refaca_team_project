using UnityEngine;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour
{
    public List<ZoneBoundary> zoneBoundaries = new List<ZoneBoundary>();
    public List<int> unlockedZoneIds = new List<int>();

    void Start()
    {
        // 초기 해금 구역 설정 (zoneId가 1인 구역)
        UnlockZone(1);
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
        if (!unlockedZoneIds.Contains(zoneId))
        {
            unlockedZoneIds.Add(zoneId);
            Debug.Log("Zone " + zoneId + " unlocked!");
            // TODO: 해금 효과 적용 및 UI 업데이트
        }
    }
}