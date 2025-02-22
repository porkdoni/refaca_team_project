using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IdleTycoon
{
    public class CropSelectionButton : MonoBehaviour
    {
        public CurrencyType cropType; // 선택할 농작물 종류
        public CurrencyAreaPrinter targetFarm; // 연결된 Farm 스크립트
        public GameObject cropSelectionPanel; // 농작물 선택 UI 패널
        //public GameObject confirmationPopup; // 확인 팝업 패널
        //public TMP_Text popupText; // 팝업 텍스트
        //public Button okButton; // 확인 버튼
        //public Button cancelButton; // 취소 버튼

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

        // 농작물 선택 처리
        void SelectCrop()
        {
            if (targetFarm != null)
            {
                // 바로 농작물 선택
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

            // 팝업 버튼 이벤트 리스너 추가
            okButton.onClick.AddListener(ConfirmCropChange);
            cancelButton.onClick.AddListener(CancelCropChange);
        }*/

        // 농작물 선택 처리
        /*void SelectCrop()
        {
            if (targetFarm != null)
            {
                // 다른 농작물을 선택하는 경우 확인 팝업 표시
                if (targetFarm.GetCurrentSelectedCrop() != CurrencyType.Select && targetFarm.GetCurrentSelectedCrop() != cropType)
                {
                    popupText.text = "Produced crops are <color=#FFFF00><size=100>reset.</size></color>\nAre you sure you want to change it?";
                    confirmationPopup.SetActive(true);
                    targetFarm.SetPopupActive(true); // 팝업 활성화
                }
                else if (targetFarm.GetCurrentSelectedCrop() == CurrencyType.Select)
                {
                    // 처음 선택하는 경우 바로 농작물 선택
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
            targetFarm.SetPopupActive(false); // 팝업 비활성화
        }

        // 농작물 변경 취소
        void CancelCropChange()
        {
            confirmationPopup.SetActive(false);
            targetFarm.SetPopupActive(false); // 팝업 비활성화
        }*/
    }
}