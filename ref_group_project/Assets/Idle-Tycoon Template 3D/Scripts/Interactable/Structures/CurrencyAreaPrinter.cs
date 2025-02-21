using Quartzified.Audio;
using System.Collections.Generic;
using UnityEngine;


namespace IdleTycoon
{
    // CurrencyAreaPrinter는 생산 주기마다 자원을 생성하여 내부에 누적시키고,
    // 그 누적된 자원을 영속적으로 저장하며, 플레이어가 상호작용할 때 자원을 전달하는 구조물입니다.
    public class CurrencyAreaPrinter : Structure
    {
        [Header("Printer Settings")]
        [Tooltip("자원이 생성될 영역 반경 (시각적 효과용)")]
        public float printArea;
        [Tooltip("생성 위치에 적용할 높이 오프셋 (UI 등 용)")]
        public float yOffset;

        /// <summary>
        /// 생산 시 참조할 위치 (이 코드에서는 시각적 효과나 UI용으로 사용 가능)
        /// </summary>
        public Vector3 spawnLocation
        {
            get
            {
                Vector3 loc = Random.onUnitSphere * printArea;
                loc += this.transform.position;
                loc.y = this.transform.position.y + yOffset;
                return loc;
            }
        }

        [Space]
        [Header("Production Settings")]
        [Tooltip("생산 주기 (초 단위)")]
        public float printFrequency = 1;
        [Tooltip("생산할 자원의 CurrencyType (예: Cash, Gem 등)")]
        public CurrencyType printType = CurrencyType.Cash;
        [Tooltip("한 생산 주기마다 생성되는 자원량")]
        public double printAmount = 1;
        [Tooltip("이 프린터가 누적할 수 있는 최대 자원량")]
        public double maxStoredAmount = 20;

        [Space]
        [Header("Audio Settings")]
        [Tooltip("구조물 잠금 해제 시 재생할 오디오 효과")]
        public EffectPack unlockEffect;

        // 생산 주기 타이머
        float printTime;

        // *** 핵심: 생산된 자원을 물리적 오브젝트 대신 내부 변수에 누적합니다.
        public double storedResource = 0;

        // 각 프린터가 고유하게 데이터를 저장하기 위한 키 (구조물 고유 ID 사용)
        // Structure.cs에 정의된 structureID가 있어야 합니다.
        private string dataKey => "Printer_" + structureID;

        // Awake()에서 PlayerPrefs(또는 다른 영속 저장소)에서 이전에 저장된 생산 데이터를 불러옵니다.
        void Awake()
        {
            // 만약 저장된 데이터가 있다면 불러와 storedResource에 할당
            if (PlayerPrefs.HasKey(dataKey))
            {
                storedResource = double.Parse(PlayerPrefs.GetString(dataKey));
            }
        }

        void Update()
        {
            // 잠금 상태라면 생산 로직을 실행하지 않습니다.
            if (!unlocked)
            {
                // 필요시 디버그 로그를 출력합니다.
                Debug.Log("Structure is locked. Production halted.");
                return;
            }

            // 생산 주기가 끝났으면 자원을 누적
            if (printTime <= 0)
            {
                Debug.Log("Production cycle complete. Adding resource.");

                storedResource += printAmount;

                // 최대 저장량을 초과하지 않도록 제한
                if (storedResource > maxStoredAmount)
                    storedResource = maxStoredAmount;

                // 생산된 자원을 PlayerPrefs에 저장하여 영속성을 확보합니다.
                PlayerPrefs.SetString(dataKey, storedResource.ToString());
                PlayerPrefs.Save();

                // 다음 생산 주기를 설정
                printTime = printFrequency;
            }
            else
            {
                printTime -= Time.deltaTime;
            }

            // UI 업데이트: 각 프린터 오브젝트의 인벤토리 상태를 표시합니다.
            UpdateUI();
        }

        // UI 요소(valueText 등)를 통해 현재 저장된 자원 정보를 표시합니다.
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
                valueText.text = storedResource.ToString("0") + "/" + maxStoredAmount.ToString("0");
            }
        }

        // 플레이어와 상호작용 시 호출됩니다.
        // 여기서 저장된 자원을 플레이어에게 전달하고, 프린터의 누적 자원을 초기화합니다.
        public override void Interact()
        {
            if (!unlocked)
                return;

            Debug.Log("Player interacted with Currency Area Printer. Transferring stored resource.");

            // 예시로 CurrencyManager를 사용하여 플레이어 자원에 추가합니다.
            CurrencyManager.Add(printType, storedResource);

            // 전달 후, 프린터의 저장 자원을 0으로 초기화하고 저장소(PlayerPrefs)에 업데이트합니다.
            storedResource = 0;
            PlayerPrefs.SetString(dataKey, storedResource.ToString());
            PlayerPrefs.Save();

            // UI 업데이트
            UpdateUI();
        }

        // 잠금 해제 시 호출되는 메서드
        protected override void OnUnlock()
        {
            printTime = printFrequency;
            unlockEffect.PlayRandom();
        }
    }
}
/*
 * namespace IdleTycoon
{
    // Currency Area Printer is a structure which spawn collectables in the surrounding area
    public class CurrencyAreaPrinter : Structure
    {
        [Header("Printer")]
        [Tooltip("Collectable spawned by the printer")]
        public CurrencyCollectable currencyObject; // Collectable Spawned 
        [Tooltip("Radius in which the Printer will place the collectables when spawned")]
        public float printArea; // Print Area
        [Tooltip("Additional offset paramater for placing height")]
        public float yOffset; // Height Offset

        /// <summary>
        /// Gets the random Spawn Location for the collectable to be placed at.
        /// </summary>
        public Vector3 spawnLocation
        {
            get
            {
                Vector3 loc = Random.onUnitSphere * printArea;
                loc += this.transform.position;
                loc.y = this.transform.position.y + yOffset;
                return loc;
            }

        }

        [Space]
        [Tooltip("Time in seconds untill the next print")]
        public float printFrequency = 1; // Seconds untill next print
        [Tooltip("CurrencyType of the printed Collectable")]
        public CurrencyType printType = CurrencyType.Cash; // Print Collectable currency Type
        [Tooltip("Currency Value of the printed Collectable")]
        public double printAmount = 1; // Value given to the printed collectable

        [Space]
        [Tooltip("Max amount of collectables that can be available at once")]
        public double maxPrintAmount = 20;

        [Header("Audio")]
        [Tooltip("AudioPack to play for when you unlock the Structure")]
        public EffectPack unlockEffect;

        // Time untill next print
        float printTime;

        // List of currently active collectables
        List<CurrencyCollectable> collectables = new List<CurrencyCollectable>();

        // Fill amount of active collectables (0f - 1f)
        float filledAmount => (float)(collectables.Count / maxPrintAmount);

        void Update()
        {
            // Checks whether the structure has been unlocked
            if (!unlocked)
                return;

            // Checks whether we should try to print the next Collectable
            if (printTime <= 0)
            {
                // Checks if we have can print more Collectables
                if (collectables.Count < maxPrintAmount)
                {
                    // Create new Collectable
                    // Sets Collectable CurrencyType & Value
                    CurrencyCollectable collectable = Instantiate(currencyObject, spawnLocation, Quaternion.identity);
                    collectable.currencyType = currencyType;
                    collectable.amount = printAmount;

                    // Add the Collectable into the tracking list
                    collectables.Add(collectable);
                }

                // Set next Print frequency
                printTime = printFrequency;
            }
            else // If not ready to print reduce print time
                printTime -= Time.deltaTime;

            // Remove all collectables that are no longer available / have been destroyed
            collectables.RemoveAll(collectable => collectable == null);

            // Update UI
            UpdateUI();
        }

        protected override void UpdateUI()
        {
            // Check if we currently have a UI element set
            if (!HasUI)
                return;

            // Check if our structure is currently locked
            if (!unlocked)
            {
                // Sets the cost of the structure if locked
                valueText.text = "Unlock Cost\n" + (unlockCost <= 0 ? "Free" : unlockCost.ToString("0"));
            }
            else
            {
                // Sets the current print fill amount to text
                valueText.text = collectables.Count.ToString("0") + "/" + maxPrintAmount.ToString("0");
            }
        }

        protected override void OnUnlock()
        {
            // Set printTime when unlocked to not instantly print the first collectable
            printTime = printFrequency;
            // Play Unlock sound
            unlockEffect.PlayRandom();
        }

    }
}
*/