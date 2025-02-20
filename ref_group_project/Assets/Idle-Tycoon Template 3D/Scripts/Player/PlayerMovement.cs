using Quartzified.Input;
using UnityEngine;
using UnityEngine.AI;

namespace IdleTycoon
{
    // PlayerMovement class handles all player related movements
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("The Player NavAgent which will be moved")]
        public NavMeshAgent agent; // Player Navagent

        Vector3 localTargetPos; // Local target position
        NavMeshPath localTargetPath; // Local target path

        private void Start()
        {
            // Set the target position to the current position
            // Set a new empty path
            localTargetPos = transform.position;
            localTargetPath = new NavMeshPath();
        }

        // Called every Fixed Update
        private void FixedUpdate()
        {
            Movement();
        }

        // Movement Function which handles all of the players movement
        void Movement()
        {
            // Get the default InputAxis Vector to determine the move direction
            Vector2 moveDir = MiniInput.InputAxisVector;

            // If we are also checking for an Input Drag Event
            // Check if this Vector is not zero and if so replace the move direction
            if(MiniInput.InputDragVector != Vector2.zero)
                moveDir = MiniInput.InputDragVector;

            // If move direction is not zero then we are moving
            if(moveDir != Vector2.zero)
            {
                // Calculate the new move point / move to position
                Vector3 movePoint = this.transform.position + new Vector3(moveDir.x, 0, moveDir.y);

                NavMeshHit navHit; // NavMesh hit for Agent
                // Sample the possible NavMesh Position and return as navHit
                if (NavMesh.SamplePosition(movePoint, out navHit, 2f, NavMesh.AllAreas))
                {
                    // If found a valid position of the NavMesh
                    // Calculate the Path to the position and set it as our local target position
                    NavMesh.CalculatePath(this.transform.position, navHit.position, NavMesh.AllAreas, localTargetPath);
                    localTargetPos = localTargetPath.corners[localTargetPath.corners.Length - 1];

                    // Set the agent destination to the nav hit position
                    agent.destination = navHit.position;
                }
            }
        }
    }
}