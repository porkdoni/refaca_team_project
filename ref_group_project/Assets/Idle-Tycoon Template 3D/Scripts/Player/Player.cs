using UnityEngine;

namespace IdleTycoon
{
    // Base Player class which handles all of the interactions with (Interactables, Collectables)
    public class Player : MonoBehaviour 
    {
        [Tooltip("Current Interactable the player is interacting with")]
        [SerializeField] Interactable curInteraction;
        public Interactable CurrentInteraction => curInteraction;

        [Space]
        [Tooltip("The rate in seconds the player interacts with Interactables")]
        public float interactionRate = 1f;

        // Time until next interaction
        float nextInteraction;

        private void Update()
        {
            // Check if we currently have an interaction
            if (curInteraction != null)
            {
                // Check if our next interaction time has passed
                if (nextInteraction <= 0)
                {
                    // Interact with the Interactable
                    curInteraction.Interact();
                    // Set the Interaction time to the interaction rate
                    nextInteraction = interactionRate;
                }
                else // Reduce interaction time
                    nextInteraction -= Time.deltaTime;
            }
        }

        // Called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            Interactable interactable; // Try get interactable from other
            if (other.gameObject.TryGetComponent(out interactable))
            {
                // If we found interactable set as current Interaction
                // Trigger OnEnterInteract on the interactable
                curInteraction = interactable;
                curInteraction.OnEnterInteract();
            }

            Collectable consumable; // Try get interactable from other
            if (other.gameObject.TryGetComponent(out consumable))
                consumable.Collect(); // Trigger Collect if we found an Collectable
        }

        // Called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            Interactable interactable; // Try get interactable from other
            if (other.gameObject.TryGetComponent(out interactable))
            {
                // Check if we currently have an interaction and if its the triggered Interactable
                if (curInteraction != null && curInteraction == interactable)
                {
                    // Trigger OnExitInteract on the interactable
                    curInteraction.OnExitInteract();

                    // Set current interaction to nothing
                    // Reset next interaction time
                    curInteraction = null;
                    nextInteraction = 0;
                }
            }
        }
    }
}