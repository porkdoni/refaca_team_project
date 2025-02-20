using UnityEngine;

namespace IdleTycoon.UI
{
    // Simple utility script that rotates an element to loot at the Main Camera
    public class UILookAtCamera : MonoBehaviour
    {
        Transform mainCamTransform;

        private void Start() => mainCamTransform = Camera.main.transform;

        private void LateUpdate()
        {
            // Checks if a main camera transform is available
            if (mainCamTransform != null)
            {
                // Look at main camera
                this.transform.LookAt(mainCamTransform);
                // Rotate UI Element by 180 to not be inverted
                this.transform.Rotate(Vector3.up * 180);
            }
        }
    }
}