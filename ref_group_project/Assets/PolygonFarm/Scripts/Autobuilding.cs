using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Autobuilding : MonoBehaviour
{
    public int buildingId;
    public int unlockCost;
    public int coinsPerMinute;
    public GameObject popupUI;
    private GameObject currentPopup;
    private bool isUnlocked = false;
    private bool isPopupActive = false;
    public GameObject costTextObject; // 인스펙터 창에서 텍스트 오브젝트 연결
    public GameObject floatingTextPrefab; // 플로팅 텍스트 프리팹 연결
    void Start()
    {
        // 게임 시작 시 해금 상태 초기화
        ResetBuildingUnlockStatus(buildingId);

        if (IsBuildingUnlocked(buildingId))
        {
            UnlockBuilding();
        }
    }

    void OnMouseDown()
    {
        if (isUnlocked) return;
        if (isPopupActive) return;

        ShowPopup();
    }

    void ShowPopup()
    {
        isPopupActive = true;
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found.");
            return;
        }
        currentPopup = Instantiate(popupUI, canvas.transform);
        currentPopup.transform.localPosition = Vector3.zero;
        currentPopup.SetActive(true);

        TextMeshProUGUI popupText = currentPopup.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        if (popupText == null)
        {
            Debug.LogError("Text (TMP) not found.");
            return;
        }

        string costText = "<color=red><size=120>" + unlockCost + " coins</size></color>";
        popupText.text = "Unlock Autobuilding " + buildingId + "?\n" + costText;

        Button confirmButton = currentPopup.transform.Find("CONFIRM").GetComponent<Button>();
        if (confirmButton == null)
        {
            Debug.LogError("CONFIRM not found.");
            return;
        }
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(UnlockBuilding);

        Button cancelButton = currentPopup.transform.Find("CANCEL").GetComponent<Button>();
        if (cancelButton == null)
        {
            Debug.LogError("CANCEL not found.");
            return;
        }
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(ClosePopup);
    }

    void UnlockBuilding()
    {
        if (FindObjectOfType<FarmManager>().coins >= unlockCost)
        {
            FindObjectOfType<FarmManager>().coins -= unlockCost;
            isUnlocked = true;
            SaveBuildingUnlockStatus(buildingId, true);
            StartCoroutine(ProduceCoins());
            ClosePopup();
            FindObjectOfType<FarmManager>().UpdateCoinUI();

            // 텍스트 오브젝트 찾아서 삭제
            if (costTextObject != null)
            {
                Destroy(costTextObject);
            }
        }
        else
        {
            Debug.Log("Not enough coins.");
        }
    }

    IEnumerator ProduceCoins()
    {
        while (isUnlocked)
        {
            yield return new WaitForSeconds(1f);
            FindObjectOfType<FarmManager>().coins += coinsPerMinute;
            FindObjectOfType<FarmManager>().UpdateCoinUI(); // 코인 UI 업데이트

            // 플로팅 텍스트 생성 및 표시
            ShowFloatingText("+" + coinsPerMinute + " coins", Color.yellow);
        }
    }

    void ShowFloatingText(string text, Color color)
    {
        if (floatingTextPrefab != null)
        {
            // 선택된 프리팹의 월드 좌표를 가져옵니다.
            Vector3 worldPosition = transform.position;

            // 플로팅 텍스트의 초기 위치를 선택된 프리팹보다 y축 5만큼 위에 설정합니다.
            Vector3 floatingTextPosition = worldPosition + new Vector3(0, 4.5f, 1f);

            // 플로팅 텍스트를 생성합니다.
            GameObject floatingText = Instantiate(floatingTextPrefab, floatingTextPosition, Quaternion.identity);

            // 플로팅 텍스트의 회전을 명시적으로 설정합니다.
            floatingText.transform.rotation = Quaternion.Euler(0, 180, 0); // 회전을 초기화하거나 원하는 축으로 회전 설정

            TextMeshPro floatingTextMesh = floatingText.GetComponent<TextMeshPro>();
            if (floatingTextMesh != null)
            {
                floatingTextMesh.text = text;
                floatingTextMesh.color = color;
                StartCoroutine(AnimateFloatingText(floatingText));

                // 디버깅 메시지 추가
                Debug.Log("플로팅 텍스트 생성: " + floatingText.name + ", 위치: " + floatingTextPosition);
            }
            else
            {
                Debug.LogError("TextMeshPro 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("플로팅 텍스트 프리팹이 연결되지 않았습니다.");
        }
    }

    IEnumerator AnimateFloatingText(GameObject floatingText)
    {
        TextMeshPro floatingTextMesh = floatingText.GetComponent<TextMeshPro>();
        Vector3 initialPosition = floatingText.transform.position;
        float elapsedTime = 0f;
        float duration = 1f; // 애니메이션 지속 시간

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // y축으로 이동 (3에서 4로)
            floatingText.transform.position = initialPosition + new Vector3(0, Mathf.Lerp(0, 1f, t), 0);

            // 알파 값 감소
            Color color = floatingTextMesh.color;
            color.a = 1 - t;
            floatingTextMesh.color = color;

            yield return null;
        }

        Destroy(floatingText);
    }


    void ClosePopup()
    {
        isPopupActive = false;
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
        }
    }

    void SaveBuildingUnlockStatus(int buildingId, bool unlocked)
    {
        PlayerPrefs.SetInt("Autobuilding_" + buildingId, unlocked ? 1 : 0);
    }

    bool IsBuildingUnlocked(int buildingId)
    {
        return PlayerPrefs.GetInt("Autobuilding_" + buildingId, 0) == 1;
    }

    // 게임 시작 시 해금 상태 초기화 함수
    void ResetBuildingUnlockStatus(int buildingId)
    {
        PlayerPrefs.SetInt("Autobuilding_" + buildingId, 0);
    }
}