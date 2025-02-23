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
    public GameObject costTextObject; // �ν����� â���� �ؽ�Ʈ ������Ʈ ����
    public GameObject floatingTextPrefab; // �÷��� �ؽ�Ʈ ������ ����
    void Start()
    {
        // ���� ���� �� �ر� ���� �ʱ�ȭ
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

            // �ؽ�Ʈ ������Ʈ ã�Ƽ� ����
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
            FindObjectOfType<FarmManager>().UpdateCoinUI(); // ���� UI ������Ʈ

            // �÷��� �ؽ�Ʈ ���� �� ǥ��
            ShowFloatingText("+" + coinsPerMinute + " coins", Color.yellow);
        }
    }

    void ShowFloatingText(string text, Color color)
    {
        if (floatingTextPrefab != null)
        {
            // ���õ� �������� ���� ��ǥ�� �����ɴϴ�.
            Vector3 worldPosition = transform.position;

            // �÷��� �ؽ�Ʈ�� �ʱ� ��ġ�� ���õ� �����պ��� y�� 5��ŭ ���� �����մϴ�.
            Vector3 floatingTextPosition = worldPosition + new Vector3(0, 4.5f, 1f);

            // �÷��� �ؽ�Ʈ�� �����մϴ�.
            GameObject floatingText = Instantiate(floatingTextPrefab, floatingTextPosition, Quaternion.identity);

            // �÷��� �ؽ�Ʈ�� ȸ���� ��������� �����մϴ�.
            floatingText.transform.rotation = Quaternion.Euler(0, 180, 0); // ȸ���� �ʱ�ȭ�ϰų� ���ϴ� ������ ȸ�� ����

            TextMeshPro floatingTextMesh = floatingText.GetComponent<TextMeshPro>();
            if (floatingTextMesh != null)
            {
                floatingTextMesh.text = text;
                floatingTextMesh.color = color;
                StartCoroutine(AnimateFloatingText(floatingText));

                // ����� �޽��� �߰�
                Debug.Log("�÷��� �ؽ�Ʈ ����: " + floatingText.name + ", ��ġ: " + floatingTextPosition);
            }
            else
            {
                Debug.LogError("TextMeshPro ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("�÷��� �ؽ�Ʈ �������� ������� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator AnimateFloatingText(GameObject floatingText)
    {
        TextMeshPro floatingTextMesh = floatingText.GetComponent<TextMeshPro>();
        Vector3 initialPosition = floatingText.transform.position;
        float elapsedTime = 0f;
        float duration = 1f; // �ִϸ��̼� ���� �ð�

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // y������ �̵� (3���� 4��)
            floatingText.transform.position = initialPosition + new Vector3(0, Mathf.Lerp(0, 1f, t), 0);

            // ���� �� ����
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

    // ���� ���� �� �ر� ���� �ʱ�ȭ �Լ�
    void ResetBuildingUnlockStatus(int buildingId)
    {
        PlayerPrefs.SetInt("Autobuilding_" + buildingId, 0);
    }
}