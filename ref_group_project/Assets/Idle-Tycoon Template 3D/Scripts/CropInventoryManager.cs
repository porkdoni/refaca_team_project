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

            // ���۹� ������ �ִ� ���� �ʱ�ȭ
            maxStorage[CurrencyType.Apple] = 100;
            maxStorage[CurrencyType.Banana] = 150;
            maxStorage[CurrencyType.Carrot] = 200;
            maxStorage[CurrencyType.Eggplant] = 250;
        }

        // ���۹� �κ��丮 (�� CropType �� ���� ����)
        private Dictionary<CurrencyType, double> crops = new Dictionary<CurrencyType, double>();

        // �ִ� ���� ���� ���� (�ʿ信 ���� ���� ����)
        private Dictionary<CurrencyType, double> maxStorage = new Dictionary<CurrencyType, double>();

        /// <summary>
        /// ���۹��� �߰��մϴ�. �ִ� ������ �ʰ��ϸ� �� �̻� �߰����� �ʽ��ϴ�.
        /// </summary>
        public bool AddItem(CurrencyType crop, double amount)
        {
            if (!crops.ContainsKey(crop))
                crops[crop] = 0; // ó�� �߰��� ��� �ʱ�ȭ

            // ���� ������ �߰��� ������ ������ �� �ִ� ������ �ʰ��ϴ��� Ȯ��
            if (crops[crop] + amount > maxStorage[crop])
            {
                crops[crop] = maxStorage[crop]; // �ִ� ���������� ����
                return false; // MAX ����
            }

            crops[crop] += amount; // ���������� �߰�
            return true; // ���� �߰���
        }

        /// <summary>
        /// ���۹��� ����մϴ�.
        /// </summary>
        public bool UseItem(CurrencyType crop, double amount)
        {
            if (crops.ContainsKey(crop) && crops[crop] >= amount)
            {
                crops[crop] -= amount;
                return true; // ���� ����
            }
            return false; // �����Ͽ� ��� �Ұ�
        }

        /// <summary>
        /// ���� ���۹� ������ ��ȯ�մϴ�.
        /// </summary>
        public double GetAmount(CurrencyType crop)
        {
            return crops.ContainsKey(crop) ? crops[crop] : 0;
        }

        /// <summary>
        /// �ִ� ���� ������ �����մϴ�.
        /// </summary>
        public void SetMaxStorage(CurrencyType crop, double maxAmount)
        {
            maxStorage[crop] = maxAmount;
        }

        /// <summary>
        /// ���� ���۹��� �ִ� ������ �����ߴ��� Ȯ���մϴ�.
        /// </summary>
        public bool IsMax(CurrencyType crop)
        {
            return crops.ContainsKey(crop) && crops[crop] >= maxStorage[crop];
        }
    }
}