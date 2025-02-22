using UnityEngine;

namespace IdleTycoon
{
    // Base abstract class for all collectables to use.
    public abstract class Collectable : MonoBehaviour
    {
        // Collect function which is triggered when a player interacts with it.
        public abstract void Collect();
    }
}
