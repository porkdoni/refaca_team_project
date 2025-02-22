using Quartzified.Input;
using UnityEngine;
using UnityEngine.AI;

namespace IdleTycoon
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("The Player NavAgent which will be moved")]
        public NavMeshAgent agent; // Player Navagent

        Vector3 localTargetPos; // Local target position
        NavMeshPath localTargetPath; // Local target path

        public static bool IsUIActive = false; //  UI�� Ȱ��ȭ�Ǿ����� üũ�ϴ� ���� �߰�

        private void Start()
        {
            localTargetPos = transform.position;
            localTargetPath = new NavMeshPath();
        }

        private void FixedUpdate()
        {
            if (!IsUIActive) //  UI�� Ȱ��ȭ�� ���¿����� �̵����� ����
                Movement();
        }

        void Movement()
        {
            Vector2 moveDir = MiniInput.InputAxisVector;

            if (MiniInput.InputDragVector != Vector2.zero)
                moveDir = MiniInput.InputDragVector;

            if (moveDir != Vector2.zero)
            {
                Vector3 movePoint = this.transform.position + new Vector3(moveDir.x, 0, moveDir.y);

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(movePoint, out navHit, 2f, NavMesh.AllAreas))
                {
                    NavMesh.CalculatePath(this.transform.position, navHit.position, NavMesh.AllAreas, localTargetPath);
                    localTargetPos = localTargetPath.corners[localTargetPath.corners.Length - 1];

                    agent.destination = navHit.position;
                }
            }
        }
    }
}