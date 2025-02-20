using Quartzified.Audio;
using TMPro;
using UnityEngine;

namespace IdleTycoon
{
    
    public class CurrencyCollectable : Collectable
    {
        public CurrencyType currencyType;
        public double amount;

        [Space]
        [Tooltip("Text Popup element for when you pick up the collectable")]
        public GameObject gainPop;
        [Space]
        [Tooltip("AudioPack to play for when you pick up the collectable")]
        public EffectPack audioPack;

        // Collect function called when player interacts with collectable
        public override void Collect()
        {
            // Add currency of type and amount
            CurrencyManager.Add(currencyType, amount);

            // Play audio if available
            if (audioPack != null)
                audioPack.PlayRandom();

            // Show Gain Pop if available
            if (gainPop != null)
            {
                // Spawn Gain Pop at the collectables position
                GameObject pop = Instantiate(gainPop, this.transform.position, Quaternion.identity);
                // Set the Gain Pops text to the amount gained
                pop.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount.ToString("0");

                // Destroy pop after 2 seconds
                Destroy(pop, 2f);
            }

            // Destroy collectable once used
            // TODO make poolable
            Destroy(this.gameObject);
        }
    }
}
