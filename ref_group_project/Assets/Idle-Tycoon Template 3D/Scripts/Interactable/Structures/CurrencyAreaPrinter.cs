using Quartzified.Audio;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

namespace IdleTycoon
{
    public class CurrencyAreaPrinter : Structure
    {
        [Header("Crop Production Settings")]
        [SerializeField]
        private List<CropProductionData> cropProductionDataList = new List<CropProductionData>()
        {
            new CropProductionData { cropType = CurrencyType.Apple, productionPerHour = 10, maxStorage = 100 },
            new CropProductionData { cropType = CurrencyType.Banana, productionPerHour = 15, maxStorage = 150 },
            new CropProductionData { cropType = CurrencyType.Carrot, productionPerHour = 20, maxStorage = 200 },
            new CropProductionData { cropType = CurrencyType.Eggplant, productionPerHour = 25, maxStorage = 250 }
        };

        private Dictionary<CurrencyType, double> productionPerHourDict = new Dictionary<CurrencyType, double>();
        private Dictionary<CurrencyType, double> maxStorageDict = new Dictionary<CurrencyType, double>();

        [Serializable]
        private class CropProductionData
        {
            public CurrencyType cropType;
            public double productionPerHour;
            public double maxStorage;
        }
        void Awake()
        {
            // 딕셔너리 초기화
            foreach (var data in cropProductionDataList)
            {
                productionPerHourDict[data.cropType] = data.productionPerHour;
                maxStorageDict[data.cropType] = data.maxStorage;
            }
        }

        [Header("Audio")]
        [Tooltip("구조물 잠금 해제 효과음")]
        public EffectPack unlockEffect;

        [Header("New Field Prefab")]
        public GameObject newFieldPrefab; // 새로운 밭 프리팹 연결

        [Header("UI")]

        private float productionTimer = 0;
        private CurrencyType selectedCrop = CurrencyType.Select; // 선택된 농작물 종류
        private CurrencyType currentSelectedCrop = CurrencyType.Select; // 현재 선택된 농작물 종류

        [Header("Crop Prefabs")]
        public GameObject applePrefab;
        public GameObject bananaPrefab;
        public GameObject carrotPrefab;
        public GameObject eggplantPrefab;

        [Header("Visuals")]
        public GameObject visuals; // Visuals 게임 오브젝트 연결

        private bool isPopupActive = false; // 팝업 활성화 상태

        void Start()
        {
            currentSelectedCrop = CurrencyType.Select;
            // 인벤토리에서 최대 개수 설정
            if (selectedCrop != CurrencyType.Select)
            {
                CropInventoryManager.Instance.SetMaxStorage(selectedCrop, maxStorageDict[selectedCrop]);
            }

            // 시작 시 모든 농작물 프리팹 비활성화
            DeactivateAllCropPrefabs();

            // 해금된 경우 새로운 밭 프리팹 활성화 및 "Select" 텍스트 표시
            if (unlocked)
            {
                ActivateNewFieldPrefab();
            }
        }

        void Update()
        {
            // 건물이 잠겨 있거나 농작물이 선택되지 않았으면 아무 동작도 하지 않음
            if (!unlocked || selectedCrop == CurrencyType.Select)
                return;

            // 이미 MAX 상태면 생산 중단
            if (CropInventoryManager.Instance.IsMax(selectedCrop))
                return;

            // 현재 선택된 농작물 종류에 해당하는 생산량 가져오기
            double currentProductionPerHour = productionPerHourDict[selectedCrop];

            // 1시간당 생산량을 기준으로 초 단위 변환
            float productionPerSecond = (float)(currentProductionPerHour / 3600);
            productionTimer += Time.deltaTime;

            // 생산 시간 계산
            if (productionTimer >= 1f) // 매 1초마다 생산 체크
            {
                double produceAmount = productionPerSecond * productionTimer;
                productionTimer = 0;

                // 농작물 추가 (최대 개수 초과 시 중단)
                bool added = CropInventoryManager.Instance.AddItem(selectedCrop, produceAmount);

                // MAX 상태이면 더 이상 생산하지 않음
                if (!added)
                    Debug.Log($"{selectedCrop} Crop Full! (MAX)");
            }

            // UI 업데이트
            UpdateUI();
        }

        protected override void UpdateUI()
        {
            if (!HasUI)
                return;

            if (!unlocked)
            {
                valueText.text = "Unlock Cost\n" + (unlockCost <= 0 ? "Free" : unlockCost.ToString("0"));
            }
            else
            {
                if (selectedCrop == CurrencyType.Select)
                {
                    // 농작물이 선택되지 않은 경우 "Select" 텍스트 표시
                    valueText.text = "Select";
                    newFieldPrefab.SetActive(true);
                }
                else
                {
                    double currentStorage = CropInventoryManager.Instance.GetAmount(selectedCrop);
                    bool isMax = CropInventoryManager.Instance.IsMax(selectedCrop);

                    // CurrencyType에 따라 농작물 이름 동적으로 표시
                    string cropName = GetCropName(selectedCrop);

                    valueText.text = isMax
                        ? $"{cropName}: MAX ({currentStorage:0}/{maxStorageDict[selectedCrop]:0})"
                        : $"{cropName}: {currentStorage:0}/{maxStorageDict[selectedCrop]:0}";
                }
            }
        }

        // CurrencyType에 따른 농작물 이름 반환
        private string GetCropName(CurrencyType crop)
        {
            switch (crop)
            {
                case CurrencyType.Apple:
                    return "Apple";
                case CurrencyType.Banana:
                    return "Banana";
                case CurrencyType.Carrot:
                    return "Carrot";
                case CurrencyType.Eggplant:
                    return "Eggplant";
                default:
                    return crop.ToString(); // 기본적으로 Enum 이름 반환
            }
        }

        protected override void OnUnlock()
        {
            productionTimer = 0; // 잠금 해제 후 생산 시작
            unlockEffect.PlayRandom(); // 잠금 해제 효과음 재생
        }

        public bool IsUnlocked()
        {
            return unlocked;
        }

        // 선택된 농작물 설정
        public void SetSelectedCrop(CurrencyType crop)
        {

            if (isPopupActive) // 팝업이 활성화된 경우 선택 방지
                return;

            if (currentSelectedCrop != CurrencyType.Select && currentSelectedCrop != crop)
            {
                // 기존 농작물 생산량 초기화
                ResetCropProduction(currentSelectedCrop);
            }

            if (currentSelectedCrop != crop)
            {
                // 선택된 농작물 종류에 따라 최대 저장 개수 설정
                if (maxStorageDict.ContainsKey(crop))
                {
                    if (CropInventoryManager.Instance != null)
                    {
                        CropInventoryManager.Instance.SetMaxStorage(crop, maxStorageDict[crop]);
                    }
                    else
                    {
                        Debug.LogError("CropInventoryManager.Instance is null!");
                    }
                }
                else
                {
                    Debug.LogError($"Max storage data not found for {crop}");
                }
            }

            currentSelectedCrop = crop; // 선택된 농작물 종류 업데이트
            selectedCrop = crop;

            Debug.Log($"Selected crop: {selectedCrop}, Current selected crop: {currentSelectedCrop}");

            // 선택된 농작물 프리팹 활성화
            ActivateSelectedCropPrefab(crop);

            // UI 업데이트
            UpdateUI();
        }

        public void SetPopupActive(bool active)
        {
            isPopupActive = active;
        }
        private void ActivateNewFieldPrefab()
        {
            if (newFieldPrefab != null)
            {
                newFieldPrefab.SetActive(true);
            }
        }

        // 새로운 밭 프리팹 비활성화
        private void DeactivateNewFieldPrefab()
        {
            if (newFieldPrefab != null)
            {
                newFieldPrefab.SetActive(false);
            }
        }

        // 기존 농작물 생산량 초기화
        private void ResetCropProduction(CurrencyType crop)
        {
            CropInventoryManager.Instance.UseItem(crop, CropInventoryManager.Instance.GetAmount(crop));
            Debug.Log($"{crop} 생산량 초기화!");
        }

        // 현재 선택된 농작물 종류 반환
        public CurrencyType GetCurrentSelectedCrop()
        {
            return currentSelectedCrop;
        }

        // 선택된 농작물 프리팹 활성화
        private void ActivateSelectedCropPrefab(CurrencyType crop)
        {
            DeactivateAllCropPrefabs();

            switch (crop)
            {
                case CurrencyType.Apple:
                    applePrefab.SetActive(true);
                    break;
                case CurrencyType.Banana:
                    bananaPrefab.SetActive(true);
                    break;
                case CurrencyType.Carrot:
                    carrotPrefab.SetActive(true);
                    break;
                case CurrencyType.Eggplant:
                    eggplantPrefab.SetActive(true);
                    break;
                default:
                    Debug.LogError($"Invalid crop type: {crop}");
                    break;
            }
        }

        // 모든 농작물 프리팹 비활성화
        private void DeactivateAllCropPrefabs()
        {
            applePrefab.SetActive(false);
            bananaPrefab.SetActive(false);
            carrotPrefab.SetActive(false);
            eggplantPrefab.SetActive(false);
        }

    }
}