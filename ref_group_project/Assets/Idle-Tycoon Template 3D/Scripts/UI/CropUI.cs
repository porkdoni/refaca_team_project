using UnityEngine;
using UnityEngine.UI;

public class CropUI : MonoBehaviour
{
    public GameObject panel; // ��Ȱ��ȭ�� UI �г�
    public Button closeButton; // �г��� ���� ��ư

    void Start()
    {
        // ��ư �̺�Ʈ ����
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    // �г� ��Ȱ��ȭ �Լ�
    void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
