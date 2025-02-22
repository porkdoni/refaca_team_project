using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IdleTycoon
{
    public class CropSelectionButton : MonoBehaviour
    {
        public CurrencyType cropType; // ������ ���۹� ����
        public CurrencyAreaPrinter targetFarm; // ����� Farm ��ũ��Ʈ
        public GameObject cropSelectionPanel; // ���۹� ���� UI �г�
        //public GameObject confirmationPopup; // Ȯ�� �˾� �г�
        //public TMP_Text popupText; // �˾� �ؽ�Ʈ
        //public Button okButton; // Ȯ�� ��ư
        //public Button cancelButton; // ��� ��ư

        private Button button;

        void Start()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(SelectCrop);
            }
            if (targetFarm == null)
            {
                Debug.LogError("targetFarm is not assigned!");
            }
        }

        // ���۹� ���� ó��
        void SelectCrop()
        {
            if (targetFarm != null)
            {
                // �ٷ� ���۹� ����
                targetFarm.SetSelectedCrop(cropType);
                cropSelectionPanel.SetActive(false);
            }
        }

        /*void Start()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(SelectCrop);
            }
            if (targetFarm == null)
            {
                Debug.LogError("targetFarm is not assigned!");
            }

            // �˾� ��ư �̺�Ʈ ������ �߰�
            okButton.onClick.AddListener(ConfirmCropChange);
            cancelButton.onClick.AddListener(CancelCropChange);
        }*/

        // ���۹� ���� ó��
        /*void SelectCrop()
        {
            if (targetFarm != null)
            {
                // �ٸ� ���۹��� �����ϴ� ��� Ȯ�� �˾� ǥ��
                if (targetFarm.GetCurrentSelectedCrop() != CurrencyType.Select && targetFarm.GetCurrentSelectedCrop() != cropType)
                {
                    popupText.text = "Produced crops are <color=#FFFF00><size=100>reset.</size></color>\nAre you sure you want to change it?";
                    confirmationPopup.SetActive(true);
                    targetFarm.SetPopupActive(true); // �˾� Ȱ��ȭ
                }
                else if (targetFarm.GetCurrentSelectedCrop() == CurrencyType.Select)
                {
                    // ó�� �����ϴ� ��� �ٷ� ���۹� ����
                    targetFarm.SetSelectedCrop(cropType);
                    cropSelectionPanel.SetActive(false);
                }
            }
        }

          void ConfirmCropChange()
        {
            Debug.Log($"Confirming crop change to: {cropType}");
            targetFarm.SetSelectedCrop(cropType);
            cropSelectionPanel.SetActive(false);
            confirmationPopup.SetActive(false);
            targetFarm.SetPopupActive(false); // �˾� ��Ȱ��ȭ
        }

        // ���۹� ���� ���
        void CancelCropChange()
        {
            confirmationPopup.SetActive(false);
            targetFarm.SetPopupActive(false); // �˾� ��Ȱ��ȭ
        }*/
    }
}