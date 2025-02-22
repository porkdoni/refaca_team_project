using UnityEngine;
using UnityEngine.UI;

public class CropUI : MonoBehaviour
{
    public GameObject panel; // 비활성화할 UI 패널
    public Button closeButton; // 패널을 닫을 버튼

    void Start()
    {
        // 버튼 이벤트 연결
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    // 패널 비활성화 함수
    void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
