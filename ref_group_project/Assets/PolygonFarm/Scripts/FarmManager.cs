using UnityEngine;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour
{
    public List<ZoneBoundary> zoneBoundaries = new List<ZoneBoundary>();
    public List<int> unlockedZoneIds = new List<int>();

    void Start()
    {
        // �ʱ� �ر� ���� ���� (zoneId�� 1�� ����)
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
            // TODO: �ر� ȿ�� ���� �� UI ������Ʈ
        }
    }
}