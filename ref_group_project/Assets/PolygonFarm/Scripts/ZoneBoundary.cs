using UnityEngine;

public class ZoneBoundary : MonoBehaviour
{
    public int zoneId;
    public Rect bounds;

    void Start()
    {
        bounds = new Rect(transform.position.x - transform.localScale.x / 2, transform.position.z - transform.localScale.z / 2, transform.localScale.x, transform.localScale.z);
    }

    void OnMouseDown()
    {
        FindObjectOfType<FarmManager>().UnlockZone(zoneId);
    }
}