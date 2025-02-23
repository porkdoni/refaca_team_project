using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockZoneObject : MonoBehaviour
{
    public int zoneIdToUnlock;
    public GameObject popupUI;
    private GameObject currentPopup;
    private bool isPopupActive = false;
    public Color costTextColor = Color.red;
    public int costTextFontSize = 120;
    public int areaNumberFontSize = 110;

    public void SetZoneIdToUnlock(int zoneId)
    {
        zoneIdToUnlock = zoneId;
    }

    void OnMouseDown()
    {
        if (isPopupActive) return;
        Debug.Log("OnMouseDown() called.");
        ShowPopup();
    }

    void ShowPopup()
    {
        if (isPopupActive) return;
        isPopupActive = true;
        FindObjectOfType<QuarterViewCamera>().isPopupActive = true;

        Debug.Log("Popup UI instantiated.");
        Debug.Log("ShowPopup() called.");
        if (popupUI != null)
        {
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

            ZoneBoundary zoneBoundary = FindObjectOfType<FarmManager>().zoneBoundaries.Find(z => z.zoneId == zoneIdToUnlock);
            if (zoneBoundary != null)
            {
                string areaText = "<size=" + areaNumberFontSize + ">" + zoneIdToUnlock.ToString() + "</size> area";
                string costText = "<color=#" + ColorUtility.ToHtmlStringRGB(costTextColor) + "><size=" + costTextFontSize + ">" + zoneBoundary.unlockCost + " coins</size></color>";
                popupText.text = "Would you like to unlock " + areaText + "?\n" + costText;
            }
            else
            {
                popupText.text = "Would you like to unlock this area?";
            }

            Button confirmButton = currentPopup.transform.Find("CONFIRM").GetComponent<Button>();
            if (confirmButton == null)
            {
                Debug.LogError("CONFIRM not found.");
                return;
            }
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(ConfirmUnlock);

            Button cancelButton = currentPopup.transform.Find("CANCEL").GetComponent<Button>();
            if (cancelButton == null)
            {
                Debug.LogError("CANCEL not found.");
                return;
            }
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(ClosePopup);

            Debug.Log("Popup UI instantiated.");
        }
        else
        {
            Debug.LogError("Popup UI prefab is not assigned or already active.");
        }
    }

    void ConfirmUnlock()
    {
        Debug.Log("ConfirmUnlock() called.");
        FarmManager farmManager = FindObjectOfType<FarmManager>();
        ZoneBoundary zoneBoundary = farmManager.zoneBoundaries.Find(z => z.zoneId == zoneIdToUnlock);

        if (zoneBoundary != null && farmManager.coins >= zoneBoundary.unlockCost)
        {
            farmManager.UnlockZone(zoneIdToUnlock);
            Destroy(gameObject);
            ClosePopup();
        }
        else
        {
            Debug.Log("Not enough coins to unlock zone.");
            // 팝업 종료 막기 (ClosePopup() 호출 안함)
        }
    }

    void ClosePopup()
    {
        isPopupActive = false;
        FindObjectOfType<QuarterViewCamera>().isPopupActive = false;

        Debug.Log("ClosePopup() called.");
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
            Debug.Log("Popup UI destroyed.");
        }
    }
}