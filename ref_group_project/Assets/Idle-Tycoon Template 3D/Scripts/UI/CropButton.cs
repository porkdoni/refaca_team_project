using UnityEngine;

namespace IdleTycoon
{
    public class CropButton : MonoBehaviour
    {
        public GameObject cropSelectionPanel; // 농작물 선택 UI 패널
        public CurrencyAreaPrinter targetFarm; // 연결할 Farm 스크립트

        void Start()
        {
            // 시작 시 패널 비활성화 (선택 사항)
            if (cropSelectionPanel != null)
            {
                cropSelectionPanel.SetActive(false);
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider == GetComponent<SphereCollider>())
                    {
                        // Sphere Collider가 클릭되었을 때 실행할 코드
                        Debug.Log("Sphere Button Clicked!");

                        // Farm이 잠금 해제되었는지 확인
                        if (targetFarm != null && targetFarm.IsUnlocked())
                        {
                            // 농작물 선택 UI 패널 활성화
                            if (cropSelectionPanel != null)
                            {
                                cropSelectionPanel.SetActive(true);
                            }
                        }
                        else
                        {
                            Debug.Log("Farm is locked!");
                        }
                    }
                }
            }
        }
    }
}