using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockZoneObject : MonoBehaviour
{
    public int zoneIdToUnlock;
    public GameObject popupUI; // ÆË¾÷ UI ÇÁ¸®ÆÕ
    private GameObject currentPopup; // ÇöÀç È°¼ºÈ­µÈ ÆË¾÷
    private bool isPopupActive = false; // ÆË¾÷ UI È°¼ºÈ­ ¿©ºÎ

    void OnMouseDown()
    {
        if (!isPopupActive)
        {
            Debug.Log("OnMouseDown() called.");
            ShowPopup();
        }
    }

    void ShowPopup()
    {
        Debug.Log("ShowPopup() called.");
        if (popupUI != null && !isPopupActive)
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
            popupText.text = "Would you like to unlock area " + zoneIdToUnlock + "?";

            Button confirmButton = currentPopup.transform.Find("CONFIRM").GetComponent<Button>();
            if (confirmButton == null)
            {
                Debug.LogError("CONFIRM not found.");
                return;
            }
            confirmButton.onClick.AddListener(ConfirmUnlock);

            Button cancelButton = currentPopup.transform.Find("CANCEL").GetComponent<Button>();
            if (cancelButton == null)
            {
                Debug.LogError("CANCEL not found.");
                return;
            }
            cancelButton.onClick.AddListener(ClosePopup);

            isPopupActive = true;
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
        FindObjectOfType<FarmManager>().UnlockZone(zoneIdToUnlock);
        Destroy(gameObject);
        ClosePopup();
    }

    void ClosePopup()
    {
        Debug.Log("ClosePopup() called.");
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
            Debug.Log("Popup UI destroyed.");
        }
        isPopupActive = false;
        ToggleUIInteraction(true);
        FindObjectOfType<QuarterViewCamera>().isPopupActive = false;
    }

    void ToggleUIInteraction(bool enable)
    {
        CanvasGroup[] canvasGroups = FindObjectsOfType<CanvasGroup>();
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            if (canvasGroup.gameObject != currentPopup)
            {
                canvasGroup.interactable = enable;
                canvasGroup.blocksRaycasts = enable;
            }
        }
    }
}