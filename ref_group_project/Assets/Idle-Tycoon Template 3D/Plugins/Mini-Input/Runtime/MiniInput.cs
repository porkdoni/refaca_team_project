using UnityEngine;

namespace Quartzified.Input
{
    public class MiniInput : MonoBehaviour
    {
        static MiniInput instance;
        public static MiniInput Instance => instance;

        [Header("Active Input Types")]
        public InputTypes activeInputTypes = InputTypes.InputAxis | InputTypes.MouseDrag;

        [Header("Drag Visual")]
        public RectTransform dragArea;
        public RectTransform dragHandle;

        public static Vector2 InputAxisVector;
        public static Vector2 InputDragVector;

        static Vector3 dragStart;
        static bool isDragging;

        private void Awake()
        {
            instance = instance ?? this;
            if (instance != this)
            {
                DestroyImmediate(this);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            if ((activeInputTypes & InputTypes.InputAxis) != 0)
                HandleInputAxis();

            if((activeInputTypes & InputTypes.MouseDrag) != 0)
                HandleMouseDrag();

        }

        void HandleInputAxis()
        {
            InputAxisVector.x = UnityEngine.Input.GetAxis("Horizontal");
            InputAxisVector.y = UnityEngine.Input.GetAxis("Vertical");
        }

        void HandleMouseDrag()
        {
            if(!isDragging)
            {
                if(dragArea)
                    dragArea.gameObject.SetActive(false);

                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                    dragStart = UnityEngine.Input.mousePosition;
                }
            }
            else
            {
                InputDragVector = (UnityEngine.Input.mousePosition - dragStart).normalized;

                if (dragArea)
                {
                    // If we are dragging we enable the dragArea and set it to the dragStart position.
                    dragArea.gameObject.SetActive(true);
                    dragArea.position = dragStart;

                    // Calculate the radius of the dragArea
                    float radius = (dragArea.rect.width / 2f);

                    // Normalize the InputDragVector and multiply by the radius to get the desired position
                    Vector2 desiredPos = InputDragVector * radius;

                    // Ensure the desired position does not exceed the radius
                    desiredPos = Vector2.ClampMagnitude(desiredPos, radius);

                    // Convert the local position to world position
                    Vector3 worldPos = dragArea.TransformPoint(desiredPos);

                    // Set the position of the dragHandle
                    dragHandle.position = worldPos;
                }

                if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                    InputDragVector = Vector2.zero;
                }
            }

        }
    }

}
