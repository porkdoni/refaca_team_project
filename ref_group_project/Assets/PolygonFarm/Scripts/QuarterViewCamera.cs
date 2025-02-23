using UnityEngine;

public class QuarterViewCamera : MonoBehaviour
{
    public FarmManager farmManager;
    public float moveSpeed = 5f;
    public float zoomSpeed = 5f;
    public float minZoom = 30f;
    public float maxZoom = 60f;

    private Vector3 mouseStartPos;
    private Vector3 cameraStartPos;
    private bool isDragging = false;
    private Camera mainCamera;
    public bool isPopupActive = false; // 팝업 활성화 여부

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        mainCamera.fieldOfView = 45f;
    }

    void Update()
    {
        if (farmManager.unlockedZoneIds.Count == 0 || isPopupActive)
        {
            return;
        }

        if (farmManager.unlockedZoneIds.Count == 0)
        {
            return;
        }

        // 마우스 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
            cameraStartPos = transform.position;
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - mouseStartPos;
            Vector3 moveDirection = new Vector3(-mouseDelta.x, 0, -mouseDelta.y) * moveSpeed;
            Vector3 targetPosition = cameraStartPos + moveDirection;

            // 카메라 위치 제한 로직 개선
            targetPosition = ClampTargetPositionToUnlockedZones(targetPosition);

            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        ClampCameraPositionToUnlockedZones();

        // 줌 처리
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.fieldOfView -= scroll * zoomSpeed;
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minZoom, maxZoom);

        // 줌 후 카메라 위치 제한
        ClampCameraPositionToUnlockedZones();
    }

    void ClampCameraPositionToUnlockedZones()
    {
        if (!farmManager.IsPositionInUnlockedZone(transform.position))
        {
            Vector3 clampedPosition = GetClampedPositionToUnlockedZones(transform.position);
            transform.position = Vector3.Lerp(transform.position, clampedPosition, 0.1f);
        }
    }

    Vector3 ClampTargetPositionToUnlockedZones(Vector3 targetPosition)
    {
        if (!farmManager.IsPositionInUnlockedZone(targetPosition))
        {
            Vector3 clampedPosition = targetPosition;
            foreach (int zoneId in farmManager.unlockedZoneIds)
            {
                ZoneBoundary zoneBoundary = farmManager.zoneBoundaries.Find(z => z.zoneId == zoneId);
                if (zoneBoundary != null)
                {
                    clampedPosition.x = Mathf.Clamp(targetPosition.x, zoneBoundary.bounds.xMin, zoneBoundary.bounds.xMax);
                    clampedPosition.z = Mathf.Clamp(targetPosition.z, zoneBoundary.bounds.yMin, zoneBoundary.bounds.yMax);
                    break;
                }
            }
            return clampedPosition;
        }
        return targetPosition;
    }

    Vector3 GetClampedPositionToUnlockedZones(Vector3 position)
    {
        Vector3 clampedPosition = position;
        foreach (int zoneId in farmManager.unlockedZoneIds)
        {
            ZoneBoundary zoneBoundary = farmManager.zoneBoundaries.Find(z => z.zoneId == zoneId);
            if (zoneBoundary != null)
            {
                clampedPosition.x = Mathf.Clamp(position.x, zoneBoundary.bounds.xMin, zoneBoundary.bounds.xMax);
                clampedPosition.z = Mathf.Clamp(position.z, zoneBoundary.bounds.yMin, zoneBoundary.bounds.yMax);
                break;
            }
        }
        return clampedPosition;
    }
}