using UnityEngine;

namespace IdleTycoon
{
    // PlayerCamera class handles all camera related movements
    public class PlayerCamera : MonoBehaviour
    {
        [Tooltip("Camera the player should controll")]
        public Camera cam;

        [Space]
        [Tooltip("The Player Transform")]
        public Transform player;
        [Tooltip("Use this to change camera target from the player to this transform instead.")]
        public Transform target;

        [Space]
        [Header("Camera Follow Speed")]
        public float speed; // Camera follow Speed
        [Header("Camera position offset")]
        public Vector3 offset;

        [Space]
        [Header("Should the camera follow its target?")]
        public bool follow = true;

        private void LateUpdate()
        {
            // Check that we have a target available
            // Check that we should be following said target
            if ((player != null || target != null) && follow)
            {
                Vector3 desPos; // Desired Position

                // Set the desired position based on follow target
                if (target != null)
                    desPos = target.position + offset;
                else
                    desPos = player.position + offset;

                // Calculate and move the camera to the desired position
                Vector3 wantPos = Vector3.Lerp(cam.transform.position, desPos, speed);
                cam.transform.position = wantPos;
            }

        }
    }
}
