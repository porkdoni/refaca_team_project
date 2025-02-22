using UnityEngine;

namespace IdleTycoon
{
    public class CropButton : MonoBehaviour
    {
        public GameObject cropSelectionPanel; // ���۹� ���� UI �г�
        public CurrencyAreaPrinter targetFarm; // ������ Farm ��ũ��Ʈ

        void Start()
        {
            // ���� �� �г� ��Ȱ��ȭ (���� ����)
            if (cropSelectionPanel != null)
            {
                cropSelectionPanel.SetActive(false);
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ�� ��
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider == GetComponent<SphereCollider>())
                    {
                        // Sphere Collider�� Ŭ���Ǿ��� �� ������ �ڵ�
                        Debug.Log("Sphere Button Clicked!");

                        // Farm�� ��� �����Ǿ����� Ȯ��
                        if (targetFarm != null && targetFarm.IsUnlocked())
                        {
                            // ���۹� ���� UI �г� Ȱ��ȭ
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