using UnityEngine;

namespace IdleTycoon
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("Interactable")]
        [Tooltip("Collider used to interact with the player")]
        public Collider triggerCollider;
        [Tooltip("LineRenderer used to showcase the interaction range")]
        public LineRenderer lineRenderer;

        /// <summary>
        /// Triggered when the Player enters the interaction
        /// </summary>
        public abstract void OnEnterInteract();
        /// <summary>
        /// Triggered when the Player leaves the interaction
        /// </summary>
        public abstract void OnExitInteract();

        /// <summary>
        /// Triggered every time the player interacts with the interactable
        /// </summary>
        public abstract void Interact();


        // Utility code to simplify LineRenderer tasks
        // Uses the Collider information to set pre determined positions for the LineRenderer
        protected virtual void OnValidate()
        {
            if (triggerCollider != null && lineRenderer != null)
            {
                if(triggerCollider is SphereCollider sphereCollider)
                {
                    Vector3[] points = MathUtil.GetCirclePoints(this.transform.position, sphereCollider.radius, 18);
                    lineRenderer.positionCount = points.Length;
                    for (int i = 0; i < points.Length; i++)
                    {
                        lineRenderer.SetPosition(i, points[i] - this.transform.position);
                    }
                }
                else
                {
                    Bounds triggerBounds = triggerCollider.bounds;
                    Vector3 extents = triggerBounds.extents;

                    float xPos = extents.x / this.transform.localScale.x;
                    float zPos = extents.z / this.transform.localScale.z;

                    lineRenderer.positionCount = 5;
                    lineRenderer.SetPosition(0, new Vector3(xPos, 0, zPos));
                    lineRenderer.SetPosition(1, new Vector3(-xPos, 0, zPos));
                    lineRenderer.SetPosition(2, new Vector3(-xPos, 0, -zPos));
                    lineRenderer.SetPosition(3, new Vector3(xPos, 0, -zPos));
                    lineRenderer.SetPosition(4, new Vector3(xPos, 0, zPos));
                }

            }
        }
    }
}
