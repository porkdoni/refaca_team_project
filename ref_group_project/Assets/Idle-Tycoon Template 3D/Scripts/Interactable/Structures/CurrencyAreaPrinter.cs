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
            // ��ųʸ� �ʱ�ȭ
            foreach (var data in cropProductionDataList)
            {
                productionPerHourDict[data.cropType] = data.productionPerHour;
                maxStorageDict[data.cropType] = data.maxStorage;
            }
        }

        [Header("Audio")]
        [Tooltip("������ ��� ���� ȿ����")]
        public EffectPack unlockEffect;

        [Header("New Field Prefab")]
        public GameObject newFieldPrefab; // ���ο� �� ������ ����

        [Header("UI")]

        private float productionTimer = 0;
        private CurrencyType selectedCrop = CurrencyType.Select; // ���õ� ���۹� ����
        private CurrencyType currentSelectedCrop = CurrencyType.Select; // ���� ���õ� ���۹� ����

        [Header("Crop Prefabs")]
        public GameObject applePrefab;
        public GameObject bananaPrefab;
        public GameObject carrotPrefab;
        public GameObject eggplantPrefab;

        [Header("Visuals")]
        public GameObject visuals; // Visuals ���� ������Ʈ ����

        private bool isPopupActive = false; // �˾� Ȱ��ȭ ����

        void Start()
        {
            currentSelectedCrop = CurrencyType.Select;
            // �κ��丮���� �ִ� ���� ����
            if (selectedCrop != CurrencyType.Select)
            {
                CropInventoryManager.Instance.SetMaxStorage(selectedCrop, maxStorageDict[selectedCrop]);
            }

            // ���� �� ��� ���۹� ������ ��Ȱ��ȭ
            DeactivateAllCropPrefabs();

            // �رݵ� ��� ���ο� �� ������ Ȱ��ȭ �� "Select" �ؽ�Ʈ ǥ��
            if (unlocked)
            {
                ActivateNewFieldPrefab();
            }
        }

        void Update()
        {
            // �ǹ��� ��� �ְų� ���۹��� ���õ��� �ʾ����� �ƹ� ���۵� ���� ����
            if (!unlocked || selectedCrop == CurrencyType.Select)
                return;

            // �̹� MAX ���¸� ���� �ߴ�
            if (CropInventoryManager.Instance.IsMax(selectedCrop))
                return;

            // ���� ���õ� ���۹� ������ �ش��ϴ� ���귮 ��������
            double currentProductionPerHour = productionPerHourDict[selectedCrop];

            // 1�ð��� ���귮�� �������� �� ���� ��ȯ
            float productionPerSecond = (float)(currentProductionPerHour / 3600);
            productionTimer += Time.deltaTime;

            // ���� �ð� ���
            if (productionTimer >= 1f) // �� 1�ʸ��� ���� üũ
            {
                double produceAmount = productionPerSecond * productionTimer;
                productionTimer = 0;

                // ���۹� �߰� (�ִ� ���� �ʰ� �� �ߴ�)
                bool added = CropInventoryManager.Instance.AddItem(selectedCrop, produceAmount);

                // MAX �����̸� �� �̻� �������� ����
                if (!added)
                    Debug.Log($"{selectedCrop} Crop Full! (MAX)");
            }

            // UI ������Ʈ
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
                    // ���۹��� ���õ��� ���� ��� "Select" �ؽ�Ʈ ǥ��
                    valueText.text = "Select";
                    newFieldPrefab.SetActive(true);
                }
                else
                {
                    double currentStorage = CropInventoryManager.Instance.GetAmount(selectedCrop);
                    bool isMax = CropInventoryManager.Instance.IsMax(selectedCrop);

                    // CurrencyType�� ���� ���۹� �̸� �������� ǥ��
                    string cropName = GetCropName(selectedCrop);

                    valueText.text = isMax
                        ? $"{cropName}: MAX ({currentStorage:0}/{maxStorageDict[selectedCrop]:0})"
                        : $"{cropName}: {currentStorage:0}/{maxStorageDict[selectedCrop]:0}";
                }
            }
        }

        // CurrencyType�� ���� ���۹� �̸� ��ȯ
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
                    return crop.ToString(); // �⺻������ Enum �̸� ��ȯ
            }
        }

        protected override void OnUnlock()
        {
            productionTimer = 0; // ��� ���� �� ���� ����
            unlockEffect.PlayRandom(); // ��� ���� ȿ���� ���
        }

        public bool IsUnlocked()
        {
            return unlocked;
        }

        // ���õ� ���۹� ����
        public void SetSelectedCrop(CurrencyType crop)
        {

            if (isPopupActive) // �˾��� Ȱ��ȭ�� ��� ���� ����
                return;

            if (currentSelectedCrop != CurrencyType.Select && currentSelectedCrop != crop)
            {
                // ���� ���۹� ���귮 �ʱ�ȭ
                ResetCropProduction(currentSelectedCrop);
            }

            if (currentSelectedCrop != crop)
            {
                // ���õ� ���۹� ������ ���� �ִ� ���� ���� ����
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

            currentSelectedCrop = crop; // ���õ� ���۹� ���� ������Ʈ
            selectedCrop = crop;

            Debug.Log($"Selected crop: {selectedCrop}, Current selected crop: {currentSelectedCrop}");

            // ���õ� ���۹� ������ Ȱ��ȭ
            ActivateSelectedCropPrefab(crop);

            // UI ������Ʈ
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

        // ���ο� �� ������ ��Ȱ��ȭ
        private void DeactivateNewFieldPrefab()
        {
            if (newFieldPrefab != null)
            {
                newFieldPrefab.SetActive(false);
            }
        }

        // ���� ���۹� ���귮 �ʱ�ȭ
        private void ResetCropProduction(CurrencyType crop)
        {
            CropInventoryManager.Instance.UseItem(crop, CropInventoryManager.Instance.GetAmount(crop));
            Debug.Log($"{crop} ���귮 �ʱ�ȭ!");
        }

        // ���� ���õ� ���۹� ���� ��ȯ
        public CurrencyType GetCurrentSelectedCrop()
        {
            return currentSelectedCrop;
        }

        // ���õ� ���۹� ������ Ȱ��ȭ
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

        // ��� ���۹� ������ ��Ȱ��ȭ
        private void DeactivateAllCropPrefabs()
        {
            applePrefab.SetActive(false);
            bananaPrefab.SetActive(false);
            carrotPrefab.SetActive(false);
            eggplantPrefab.SetActive(false);
        }

    }
}