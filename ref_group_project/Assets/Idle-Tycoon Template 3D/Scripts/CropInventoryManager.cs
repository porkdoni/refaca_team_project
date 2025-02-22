using System.Collections.Generic;
using UnityEngine;

namespace IdleTycoon
{
    public class CropInventoryManager : MonoBehaviour
    {
        public static CropInventoryManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            // 농작물 종류별 최대 개수 초기화
            maxStorage[CurrencyType.Apple] = 100;
            maxStorage[CurrencyType.Banana] = 150;
            maxStorage[CurrencyType.Carrot] = 200;
            maxStorage[CurrencyType.Eggplant] = 250;
        }

        // 농작물 인벤토리 (각 CropType 별 개수 저장)
        private Dictionary<CurrencyType, double> crops = new Dictionary<CurrencyType, double>();

        // 최대 저장 개수 설정 (필요에 따라 조정 가능)
        private Dictionary<CurrencyType, double> maxStorage = new Dictionary<CurrencyType, double>();

        /// <summary>
        /// 농작물을 추가합니다. 최대 개수를 초과하면 더 이상 추가되지 않습니다.
        /// </summary>
        public bool AddItem(CurrencyType crop, double amount)
        {
            if (!crops.ContainsKey(crop))
                crops[crop] = 0; // 처음 추가할 경우 초기화

            // 현재 개수와 추가할 개수를 합쳤을 때 최대 개수를 초과하는지 확인
            if (crops[crop] + amount > maxStorage[crop])
            {
                crops[crop] = maxStorage[crop]; // 최대 개수까지만 저장
                return false; // MAX 상태
            }

            crops[crop] += amount; // 정상적으로 추가
            return true; // 정상 추가됨
        }

        /// <summary>
        /// 농작물을 사용합니다.
        /// </summary>
        public bool UseItem(CurrencyType crop, double amount)
        {
            if (crops.ContainsKey(crop) && crops[crop] >= amount)
            {
                crops[crop] -= amount;
                return true; // 정상 사용됨
            }
            return false; // 부족하여 사용 불가
        }

        /// <summary>
        /// 현재 농작물 개수를 반환합니다.
        /// </summary>
        public double GetAmount(CurrencyType crop)
        {
            return crops.ContainsKey(crop) ? crops[crop] : 0;
        }

        /// <summary>
        /// 최대 저장 개수를 설정합니다.
        /// </summary>
        public void SetMaxStorage(CurrencyType crop, double maxAmount)
        {
            maxStorage[crop] = maxAmount;
        }

        /// <summary>
        /// 현재 농작물이 최대 개수에 도달했는지 확인합니다.
        /// </summary>
        public bool IsMax(CurrencyType crop)
        {
            return crops.ContainsKey(crop) && crops[crop] >= maxStorage[crop];
        }
    }
}