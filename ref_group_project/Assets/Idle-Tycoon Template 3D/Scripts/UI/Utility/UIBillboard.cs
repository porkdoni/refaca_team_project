using UnityEngine;

namespace IdleTycoon.UI
{
    // Simple utility script that matches an element to the main Camera's rotation
    public class UIBillboard : MonoBehaviour
    {
        Transform mainCamTransform;

        private void Start() => mainCamTransform = Camera.main.transform;

        private void LateUpdate()
        {
            // Checks if a main camera transform is available
            if (mainCamTransform != null)
            {
                // Match the rotation of the main camera to this object
                this.transform.rotation = mainCamTransform.rotation;
            }
        }
    }
}